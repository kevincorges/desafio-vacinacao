using Microsoft.EntityFrameworkCore;
using VacinacaoApi.Models;

namespace VacinacaoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Para In-Memory, o ToTable não é estritamente necessário, mas vamos manter o mapeamento
            modelBuilder.Entity<Paciente>(entity => {
                entity.HasKey(e => e.Id);
            });
            
            modelBuilder.Entity<Agendamento>(entity => {
                entity.HasKey(e => e.Id);
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
