using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacinacaoApi.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public class Agendamento
    {
        [Key]
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TimeSpan HorarioAgendamento { get; set; }
        public string Status { get; set; } = "Pendente"; // Pendente, Realizado, Cancelado
        public string Conclusao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; }
    }
}
