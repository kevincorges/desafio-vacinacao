using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using VacinacaoApi.Data;
using VacinacaoApi.Models;
using VacinacaoApi.Services;
using VacinacaoApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacinacaoApi.Tests
{
    public class AgendamentoServiceTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Criar_ComDadosValidos_DeveRetornarNull()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var dto = new CriarAgendamentoDto
            {
                Nome = "João Silva",
                DataNascimento = new DateTime(1990, 1, 15),
                DataAgendamento = new DateTime(2024, 5, 1),
                Horario = "14:00"
            };

            // Act
            var resultado = await service.Criar(dto);

            // Assert
            Assert.Null(resultado);
            var agendamentos = await context.Agendamentos.CountAsync();
            Assert.Equal(1, agendamentos);
        }

        [Fact]
        public async Task Criar_ComHorarioInvalido_DeveRetornarErro()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var dto = new CriarAgendamentoDto
            {
                Nome = "João Silva",
                DataNascimento = new DateTime(1990, 1, 15),
                DataAgendamento = new DateTime(2024, 5, 1),
                Horario = "25:00" // Inválido
            };

            // Act
            var resultado = await service.Criar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Horário inválido", resultado);
        }

        [Fact]
        public async Task Criar_ComMaisDe20AgendamentosNoDia_DeveRetornarErro()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var data = new DateTime(2024, 5, 1);

            // Criar 20 agendamentos
            for (int i = 0; i < 20; i++)
            {
                var paciente = new Paciente
                {
                    Nome = $"Paciente {i}",
                    DataNascimento = new DateTime(1990, 1, 1)
                };
                context.Pacientes.Add(paciente);
                await context.SaveChangesAsync();

                var agendamento = new Agendamento
                {
                    PacienteId = paciente.Id,
                    DataAgendamento = data,
                    HorarioAgendamento = TimeSpan.FromHours(9 + (i % 8)),
                    Status = "Pendente"
                };
                context.Agendamentos.Add(agendamento);
            }
            await context.SaveChangesAsync();

            var dto = new CriarAgendamentoDto
            {
                Nome = "Novo Paciente",
                DataNascimento = new DateTime(1990, 1, 15),
                DataAgendamento = data,
                Horario = "17:00"
            };

            // Act
            var resultado = await service.Criar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Limite de 20 agendamentos", resultado);
        }

        [Fact]
        public async Task Criar_ComMaisDe2AgendamentosNoMesmoHorario_DeveRetornarErro()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var data = new DateTime(2024, 5, 1);
            var horario = TimeSpan.FromHours(14);

            // Criar 2 agendamentos no mesmo horário
            for (int i = 0; i < 2; i++)
            {
                var paciente = new Paciente
                {
                    Nome = $"Paciente {i}",
                    DataNascimento = new DateTime(1990, 1, 1)
                };
                context.Pacientes.Add(paciente);
                await context.SaveChangesAsync();

                var agendamento = new Agendamento
                {
                    PacienteId = paciente.Id,
                    DataAgendamento = data,
                    HorarioAgendamento = horario,
                    Status = "Pendente"
                };
                context.Agendamentos.Add(agendamento);
            }
            await context.SaveChangesAsync();

            var dto = new CriarAgendamentoDto
            {
                Nome = "Terceiro Paciente",
                DataNascimento = new DateTime(1990, 1, 15),
                DataAgendamento = data,
                Horario = "14:00"
            };

            // Act
            var resultado = await service.Criar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Limite de 2 agendamentos", resultado);
        }

        [Fact]
        public async Task ListarTodos_DeveRetornarAgendamentosOrdenados()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);

            var paciente = new Paciente
            {
                Nome = "João Silva",
                DataNascimento = new DateTime(1990, 1, 15)
            };
            context.Pacientes.Add(paciente);
            await context.SaveChangesAsync();

            var agendamento1 = new Agendamento
            {
                PacienteId = paciente.Id,
                DataAgendamento = new DateTime(2024, 5, 2),
                HorarioAgendamento = TimeSpan.FromHours(14),
                Status = "Pendente"
            };
            var agendamento2 = new Agendamento
            {
                PacienteId = paciente.Id,
                DataAgendamento = new DateTime(2024, 5, 1),
                HorarioAgendamento = TimeSpan.FromHours(10),
                Status = "Pendente"
            };
            context.Agendamentos.Add(agendamento1);
            context.Agendamentos.Add(agendamento2);
            await context.SaveChangesAsync();

            // Act
            var resultado = await service.ListarTodos();

            // Assert
            Assert.Equal(2, resultado.Count());
            var lista = resultado.ToList();
            Assert.Equal(new DateTime(2024, 5, 1), lista[0].DataAgendamento);
            Assert.Equal(new DateTime(2024, 5, 2), lista[1].DataAgendamento);
        }

        [Fact]
        public async Task AtualizarStatus_ComIdValido_DeveRetornarTrue()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);

            var paciente = new Paciente
            {
                Nome = "João Silva",
                DataNascimento = new DateTime(1990, 1, 15)
            };
            context.Pacientes.Add(paciente);
            await context.SaveChangesAsync();

            var agendamento = new Agendamento
            {
                PacienteId = paciente.Id,
                DataAgendamento = new DateTime(2024, 5, 1),
                HorarioAgendamento = TimeSpan.FromHours(14),
                Status = "Pendente"
            };
            context.Agendamentos.Add(agendamento);
            await context.SaveChangesAsync();

            var dto = new AtualizarStatusDto
            {
                Status = "Realizado",
                Conclusao = "Vacinação realizada com sucesso"
            };

            // Act
            var resultado = await service.AtualizarStatus(agendamento.Id, dto);

            // Assert
            Assert.True(resultado);
            var agendamentoAtualizado = await context.Agendamentos.FindAsync(agendamento.Id);
            Assert.Equal("Realizado", agendamentoAtualizado.Status);
            Assert.Equal("Vacinação realizada com sucesso", agendamentoAtualizado.Conclusao);
        }

        [Fact]
        public async Task AtualizarStatus_ComIdInvalido_DeveRetornarFalse()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var dto = new AtualizarStatusDto
            {
                Status = "Realizado",
                Conclusao = "Teste"
            };

            // Act
            var resultado = await service.AtualizarStatus(999, dto);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Criar_ComMenosDe1HoraDeIntervalo_DeveRetornarErro()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var data = new DateTime(2024, 5, 1);
            var horario1 = TimeSpan.FromHours(14);
            var horario2 = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(30)); // 30 minutos depois

            // Criar primeiro agendamento
            var paciente1 = new Paciente
            {
                Nome = "Paciente 1",
                DataNascimento = new DateTime(1990, 1, 1)
            };
            context.Pacientes.Add(paciente1);
            await context.SaveChangesAsync();

            var agendamento1 = new Agendamento
            {
                PacienteId = paciente1.Id,
                DataAgendamento = data,
                HorarioAgendamento = horario1,
                Status = "Pendente"
            };
            context.Agendamentos.Add(agendamento1);
            await context.SaveChangesAsync();

            // Tentar criar segundo agendamento com menos de 1 hora de intervalo
            var dto = new CriarAgendamentoDto
            {
                Nome = "Paciente 2",
                DataNascimento = new DateTime(1990, 1, 15),
                DataAgendamento = data,
                Horario = "14:30"
            };

            // Act
            var resultado = await service.Criar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("intervalo de 1 hora", resultado);
        }

        [Fact]
        public async Task Criar_ComExatamente1HoraDeIntervalo_DevePermitir()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new AgendamentoService(context);
            var data = new DateTime(2024, 5, 1);
            var horario1 = TimeSpan.FromHours(14);
            var horario2 = TimeSpan.FromHours(15); // Exatamente 1 hora depois

            // Criar primeiro agendamento
            var paciente1 = new Paciente
            {
                Nome = "Paciente 1",
                DataNascimento = new DateTime(1990, 1, 1)
            };
            context.Pacientes.Add(paciente1);
            await context.SaveChangesAsync();

            var agendamento1 = new Agendamento
            {
                PacienteId = paciente1.Id,
                DataAgendamento = data,
                HorarioAgendamento = horario1,
                Status = "Pendente"
            };
            context.Agendamentos.Add(agendamento1);
            await context.SaveChangesAsync();

            // Tentar criar segundo agendamento com exatamente 1 hora de intervalo
            var dto = new CriarAgendamentoDto
            {
                Nome = "Paciente 2",
                DataNascimento = new DateTime(1990, 1, 15),
                DataAgendamento = data,
                Horario = "15:00"
            };

            // Act
            var resultado = await service.Criar(dto);

            // Assert
            Assert.Null(resultado); // Deve ser permitido
            var agendamentos = await context.Agendamentos.CountAsync();
            Assert.Equal(2, agendamentos);
        }
    }
}