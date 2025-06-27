using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Data.Builders
{
    public class AgendamentoBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agendamento>().HasKey(a => a.Id);

            modelBuilder.Entity<Agendamento>().Property(a => a.Data)
                .IsRequired();

            modelBuilder.Entity<Agendamento>().Property(a => a.Hora)
                .IsRequired();

            modelBuilder.Entity<Agendamento>().Property(a => a.Valor)
                .IsRequired();

            modelBuilder.Entity<Agendamento>().Property(a => a.Comissao)
                .IsRequired();

            modelBuilder.Entity<Agendamento>().Property(a => a.StatusAgendamento)
                .IsRequired();

            // Cliente 
            modelBuilder.Entity<Agendamento>().Property(a => a.ClienteId)
                .IsRequired();
            
            modelBuilder.Entity<Agendamento>()
                .HasOne(c => c.Cliente)
                .WithMany(a => a.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Agendamento>().Property(a => a.ProfissionalId)
                .IsRequired();

            modelBuilder.Entity<Agendamento>().HasOne(p => p. Profissional)
                .WithMany(e => e.Agendamentos)
                .HasForeignKey(p => p.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);
            

            modelBuilder.Entity<Agendamento>()
            .HasData(new List<Agendamento>
            {
                new Agendamento(1, new DateOnly(2025, 2, 5), 4, new TimeOnly(15, 17),  100.00f, 30.00f, StatusAgendamento.CONFIRMADO, 1, 1),
                new Agendamento(2, new DateOnly(2025, 1, 4), 0, new TimeOnly(21, 12), 120.00f, 36.00f, StatusAgendamento.REALIZADO, 2, 1),
                new Agendamento(3, new DateOnly(2025, 4, 2), 13, new TimeOnly(11, 30), 80.00f, 24.00f, StatusAgendamento.CANCELADO, 3, 1)
            });
        }
    }
}
