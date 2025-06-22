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
            .UsingEntity(j => j.ToTable("servico_agendamento"));
    }
}
