using Microsoft.EntityFrameworkCore;
using VacinacaoApi.Data;
using VacinacaoApi.DTOs;
using VacinacaoApi.Models;

namespace VacinacaoApi.Services
{
    public interface IAgendamentoService
    {
        Task<IEnumerable<AgendamentoResponseDto>> ListarTodos();
        Task<string> Criar(CriarAgendamentoDto dto);
        Task<bool> AtualizarStatus(int id, AtualizarStatusDto dto);
    }

    public class AgendamentoService : IAgendamentoService
    {
        private readonly AppDbContext _context;

        public AgendamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AgendamentoResponseDto>> ListarTodos()
        {
            return await _context.Agendamentos
                .Include(a => a.Paciente)
                .OrderBy(a => a.DataAgendamento)
                .ThenBy(a => a.HorarioAgendamento)
                .Select(a => new AgendamentoResponseDto
                {
                    Id = a.Id,
                    NomePaciente = a.Paciente.Nome,
                    DataNascimento = a.Paciente.DataNascimento,
                    DataAgendamento = a.DataAgendamento,
                    Horario = a.HorarioAgendamento.ToString(@"hh\:mm"),
                    Status = a.Status,
                    Conclusao = a.Conclusao
                })
                .ToListAsync();
        }

        public async Task<string> Criar(CriarAgendamentoDto dto)
        {
            // Parse do horário
            if (!TimeSpan.TryParse(dto.Horario, out var horario))
                return "Horário inválido.";

            var dataApenas = dto.DataAgendamento.Date;

            // Regra: Máximo de 20 agendamentos por dia
            var totalNoDia = await _context.Agendamentos.CountAsync(a => a.DataAgendamento.Date == dataApenas);
            if (totalNoDia >= 20)
                return "Limite de 20 agendamentos para este dia atingido.";

            // Regra: Máximo de 2 agendamentos por horário
            var totalNoHorario = await _context.Agendamentos.CountAsync(a => a.DataAgendamento.Date == dataApenas && a.HorarioAgendamento == horario);
            if (totalNoHorario >= 2)
                return "Limite de 2 agendamentos para este horário atingido.";

            // Criar ou encontrar paciente (simplificado)
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Nome == dto.Nome && p.DataNascimento.Date == dto.DataNascimento.Date);
            if (paciente == null)
            {
                paciente = new Paciente { Nome = dto.Nome, DataNascimento = dto.DataNascimento };
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
            }

            var agendamento = new Agendamento
            {
                PacienteId = paciente.Id,
                DataAgendamento = dataApenas,
                HorarioAgendamento = horario,
                Status = "Pendente"
            };

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return null; // Sucesso
        }

        public async Task<bool> AtualizarStatus(int id, AtualizarStatusDto dto)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null) return false;

            agendamento.Status = dto.Status;
            agendamento.Conclusao = dto.Conclusao;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
