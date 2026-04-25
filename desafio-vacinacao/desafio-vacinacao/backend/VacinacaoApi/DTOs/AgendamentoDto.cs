using System;

namespace VacinacaoApi.DTOs
{
    public class CriarAgendamentoDto
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string Horario { get; set; } // Formato "HH:mm"
    }

    public class AtualizarStatusDto
    {
        public string Status { get; set; }
        public string Conclusao { get; set; }
    }

    public class AgendamentoResponseDto
    {
        public int Id { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string Horario { get; set; }
        public string Status { get; set; }
        public string Conclusao { get; set; }
    }
}
