using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Builders;

public class ProfissionalBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profissional>(m =>
        {

            m.HasKey(p => p.Id);

            m.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            m.Property(p => p.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            m.Property(p => p.Salario)
                .IsRequired();

            m.Property(p => p.Login)
                .IsRequired()
                .HasMaxLength(50);

            m.Property(p => p.Senha)
                .IsRequired()
                .HasMaxLength(256);

            m.Property(p => p.Telefone)
                .IsRequired()
                .HasMaxLength(11);

            m.Property(p => p.Status)
                .IsRequired();

            m.Property(p => p.EmpresaId)
                .IsRequired();

            m.HasOne(e => e.Empresa)
                .WithMany(f => f.Profissionais)
                .HasForeignKey(f => f.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            m.HasData(new List<Profissional>
            {
                new Profissional(1, "Administrador", "00000000000", 0, "admin", "123456", "00000000000", 1, StatusProfissional.ATIVO),
                new Profissional(2, "Recepcionista", "00000000000", 0, "recepcionista", "123456", "00000000000", 1, StatusProfissional.ATIVO),
                new Profissional(3, "Profissional", "00000000000", 0, "profissional", "123456", "00000000000", 1, StatusProfissional.ATIVO)
            });

        });
    }
}