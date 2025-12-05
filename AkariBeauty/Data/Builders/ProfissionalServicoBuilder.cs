using AkariBeauty.Objects.Relationship;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Builders;

public class ProfissionalServicoBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfissionalServico>(m =>
        {
            m.HasKey(ps => new { ps.ProfissionalId, ps.ServicoId });

            m.HasOne(ps => ps.Profissional)
                .WithMany(p => p.ProfissionalServicos)
                .HasForeignKey(ps => ps.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            m.HasOne(ps => ps.Servico)
                .WithMany(s => s.ProfissionalServicos)
                .HasForeignKey(ps => ps.ServicoId)
                .OnDelete(DeleteBehavior.Cascade);

            m.Property(ps => ps.Comissao)
                .IsRequired();

            m.Property(ps => ps.Tempo)
                .IsRequired();

            m.HasData(new List<ProfissionalServico>
            {
                new ProfissionalServico(4, 1, 0.15f, new TimeOnly(0, 50, 0)),
                new ProfissionalServico(4, 12, 0.17f, new TimeOnly(1, 5, 0)),
                new ProfissionalServico(11, 11, 0.16f, new TimeOnly(1, 0, 0)),
                new ProfissionalServico(11, 13, 0.19f, new TimeOnly(1, 25, 0)),
                new ProfissionalServico(12, 2, 0.12f, new TimeOnly(0, 40, 0)),
                new ProfissionalServico(13, 14, 0.14f, new TimeOnly(0, 55, 0)),
                new ProfissionalServico(13, 15, 0.10f, new TimeOnly(0, 35, 0)),
                new ProfissionalServico(5, 4, 0.18f, new TimeOnly(1, 30, 0)),
                new ProfissionalServico(6, 3, 0.15f, new TimeOnly(1, 0, 0)),
                new ProfissionalServico(6, 7, 0.12f, new TimeOnly(0, 45, 0)),
                new ProfissionalServico(7, 3, 0.13f, new TimeOnly(1, 10, 0)),
                new ProfissionalServico(8, 6, 0.12f, new TimeOnly(0, 35, 0)),
                new ProfissionalServico(9, 5, 0.15f, new TimeOnly(1, 15, 0)),
                new ProfissionalServico(9, 9, 0.11f, new TimeOnly(0, 40, 0)),
                new ProfissionalServico(10, 10, 0.16f, new TimeOnly(1, 40, 0))
            });
        });
    }
}
