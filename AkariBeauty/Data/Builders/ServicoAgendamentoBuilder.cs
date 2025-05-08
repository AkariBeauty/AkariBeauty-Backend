using System;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Builders;

public class ServicoAgendamentoBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Servico>(entity => {
            entity.HasMany(a => a.Agendamentos)
                .WithMany(s => s.Servicos)
                .UsingEntity(t=> t.ToTable("servico_agendamento")); 
        });
    }
}
