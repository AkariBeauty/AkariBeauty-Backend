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
            modelBuilder.Entity<Agendamento>().Property(a => a.ClienteId)
                .IsRequired();
            modelBuilder.Entity<Agendamento>().Property(a => a.EmpresaId)
                .IsRequired();

            modelBuilder.Entity<Agendamento>().HasData(new List<Agendamento>
{
                new Agendamento(1, new DateTime(2025, 5, 2), new DateTime(1, 1, 1, 14, 0, 0), 100.00f, 30.00f, StatusAgendamento.CONFIRMADO, 1, 1),
                new Agendamento(2, new DateTime(2025, 5, 3), new DateTime(1, 1, 1, 15, 30, 0), 120.00f, 36.00f, StatusAgendamento.REALIZADO, 2, 1),
                new Agendamento(3, new DateTime(2025, 5, 4), new DateTime(1, 1, 1, 10, 0, 0), 80.00f, 24.00f, StatusAgendamento.CANCELADO, 3, 2)
            });
        }
    }
}
