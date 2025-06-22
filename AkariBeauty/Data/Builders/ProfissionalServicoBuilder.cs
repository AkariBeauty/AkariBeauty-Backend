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
                new ProfissionalServico(1, 1, 0.1f, new TimeOnly(0, 30, 0)),
                new ProfissionalServico(1, 2, 0.1f, new TimeOnly(0, 30, 0)),
                new ProfissionalServico(1, 3, 0.1f, new TimeOnly(0, 30, 0))
            });
        });
    }
}
