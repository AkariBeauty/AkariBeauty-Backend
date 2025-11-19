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
                .Property(a => a.Observacao)
                .HasMaxLength(500);
            
            modelBuilder.Entity<Agendamento>()
                .HasOne(c => c.Cliente)
                .WithMany(a => a.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Agendamento>()
                .HasOne(p => p.Profissional)
                .WithMany(a => a.Agendamentos)
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Agendamento>()
            .HasData(new List<Agendamento>
            {
                new Agendamento(1, new DateOnly(2025, 1, 12), new TimeOnly(10, 0), 120.00f, 36.00f, StatusAgendamento.CONFIRMADO, 1, 4, "Cliente prefere silêncio durante o procedimento."),
                new Agendamento(2, new DateOnly(2025, 1, 14), new TimeOnly(14, 30), 45.00f, 13.50f, StatusAgendamento.REALIZADO, 2, 5, null),
                new Agendamento(3, new DateOnly(2025, 1, 16), new TimeOnly(16, 0), 160.00f, 48.00f, StatusAgendamento.PENDENTE, 3, 6, "Aplicar hidratação extra."),
                new Agendamento(4, new DateOnly(2025, 1, 18), new TimeOnly(11, 15), 280.00f, 84.00f, StatusAgendamento.CONFIRMADO, 4, 7, null),
                new Agendamento(5, new DateOnly(2025, 1, 20), new TimeOnly(9, 45), 150.00f, 45.00f, StatusAgendamento.CONFIRMADO, 5, 8, null),
                new Agendamento(6, new DateOnly(2025, 1, 22), new TimeOnly(13, 30), 80.00f, 24.00f, StatusAgendamento.REALIZADO, 6, 9, null),
                new Agendamento(7, new DateOnly(2025, 1, 24), new TimeOnly(15, 0), 95.00f, 28.50f, StatusAgendamento.PENDENTE, 7, 10, null),
                new Agendamento(8, new DateOnly(2025, 1, 26), new TimeOnly(17, 15), 130.00f, 39.00f, StatusAgendamento.PENDENTE, 8, 4, null),
                new Agendamento(9, new DateOnly(2025, 1, 28), new TimeOnly(10, 30), 70.00f, 21.00f, StatusAgendamento.CONFIRMADO, 9, 5, null),
                new Agendamento(10, new DateOnly(2025, 1, 30), new TimeOnly(12, 45), 320.00f, 96.00f, StatusAgendamento.PENDENTE, 10, 6, null)
            });
        }
    }
}
