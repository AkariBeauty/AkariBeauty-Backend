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
                new Profissional(3, "Profissional", "00000000000", 0, "profissional", "123456", "00000000000", 1, StatusProfissional.ATIVO),
                new Profissional(4, "Lívia Andrade", "11122233344", 3500f, "livia.andrade", "senha123", "11987654321", 1, StatusProfissional.ATIVO),
                new Profissional(5, "Marcos Diniz", "22233344455", 3200f, "marcos.diniz", "senha123", "11912345678", 4, StatusProfissional.ATIVO),
                new Profissional(6, "Renata Freitas", "33344455566", 4000f, "renata.freitas", "senha123", "21987654321", 6, StatusProfissional.ATIVO),
                new Profissional(7, "Camila Prado", "44455566677", 3800f, "camila.prado", "senha123", "41999887766", 6, StatusProfissional.ATIVO),
                new Profissional(8, "Diego Santana", "55566677788", 3100f, "diego.santana", "senha123", "11988776655", 8, StatusProfissional.ATIVO),
                new Profissional(9, "Elaine Costa", "66677788899", 3600f, "elaine.costa", "senha123", "71977665544", 9, StatusProfissional.ATIVO),
                new Profissional(10, "Felipe Duarte", "77788899900", 3400f, "felipe.duarte", "senha123", "48966554433", 7, StatusProfissional.ATIVO),
                new Profissional(11, "Bianca Torres", "88899900011", 4200f, "bianca.torres", "senha123", "11944556677", 1, StatusProfissional.ATIVO),
                new Profissional(12, "Joao Mendes", "99900011122", 3100f, "joao.mendes", "senha123", "11977889966", 1, StatusProfissional.INATIVO),
                new Profissional(13, "Sofia Andrade", "55544433322", 3900f, "sofia.andrade", "senha123", "11955667788", 1, StatusProfissional.AFASTADO)
            });

        });
    }
}