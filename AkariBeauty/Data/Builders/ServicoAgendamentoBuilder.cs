using System.Collections.Generic;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Builders;

public class ServicoAgendamentoBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Servico>()
            .HasMany(s => s.Agendamentos)
            .WithMany(a => a.Servicos)
            .UsingEntity<Dictionary<string, object>>(
                "servico_agendamento",
                j => j.HasOne<Agendamento>().WithMany().HasForeignKey("AgendamentosId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Servico>().WithMany().HasForeignKey("ServicosId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("AgendamentosId", "ServicosId");
                    j.HasData(GetSeed());
                });
    }

    private static IEnumerable<object> GetSeed()
    {
        return new object[]
        {
            new { AgendamentosId = 1, ServicosId = 1 },
            new { AgendamentosId = 2, ServicosId = 2 },
            new { AgendamentosId = 3, ServicosId = 3 },
            new { AgendamentosId = 4, ServicosId = 5 },
            new { AgendamentosId = 5, ServicosId = 6 },
            new { AgendamentosId = 6, ServicosId = 5 },
            new { AgendamentosId = 7, ServicosId = 7 },
            new { AgendamentosId = 8, ServicosId = 4 },
            new { AgendamentosId = 9, ServicosId = 8 },
            new { AgendamentosId = 10, ServicosId = 10 },

            // Ligações dos novos agendamentos do Felipe Duarte
            new { AgendamentosId = 20, ServicosId = 10 },
            new { AgendamentosId = 21, ServicosId = 10 },
            new { AgendamentosId = 22, ServicosId = 10 },
            new { AgendamentosId = 23, ServicosId = 10 },
            new { AgendamentosId = 24, ServicosId = 10 },
            new { AgendamentosId = 25, ServicosId = 10 },
            new { AgendamentosId = 26, ServicosId = 10 },

            // Agenda corporativa da Empresa #1
            new { AgendamentosId = 30, ServicosId = 12 },
            new { AgendamentosId = 31, ServicosId = 11 },
            new { AgendamentosId = 32, ServicosId = 13 },
            new { AgendamentosId = 33, ServicosId = 2 },
            new { AgendamentosId = 34, ServicosId = 12 },
            new { AgendamentosId = 35, ServicosId = 11 },
            new { AgendamentosId = 36, ServicosId = 14 },
            new { AgendamentosId = 37, ServicosId = 13 },
            new { AgendamentosId = 38, ServicosId = 15 },
            new { AgendamentosId = 39, ServicosId = 14 }
        };
    }
}
