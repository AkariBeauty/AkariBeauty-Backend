using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Builders
{
    public class EmpresaBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empresa>().HasKey(e => e.Id);

            modelBuilder.Entity<Empresa>().Property(e => e.Cnpj)
                .IsRequired().HasMaxLength(18);

            modelBuilder.Entity<Empresa>().Property(e => e.RazaoSocial)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Empresa>().Property(e => e.Uf)
                .IsRequired().HasMaxLength(2);

            modelBuilder.Entity<Empresa>().Property(e => e.Cidade)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Empresa>().Property(e => e.Bairro)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Empresa>().Property(e => e.Rua)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Empresa>().Property(e => e.Numero)
                .IsRequired();

            modelBuilder.Entity<Empresa>().Property(e => e.HoraInicial)
                .IsRequired();

            modelBuilder.Entity<Empresa>().Property(e => e.HoraFinal)
                .IsRequired();

            modelBuilder.Entity<Empresa>().Property(e => e.Adiantamento)
                .IsRequired();

            modelBuilder.Entity<Empresa>().HasData(new List<Empresa>
            {
                new Empresa(1, "12.345.678/0001-99", "Akari Beauty Center", "SP", "São Paulo", "Centro", "Rua das Rosas", 123, new TimeOnly(8, 0), new TimeOnly(18, 0), true),
                new Empresa(2, "98.765.432/0001-88", "Estética Bela Vida", "RJ", "Rio de Janeiro", "Copacabana", "Avenida Atlântica", 456, new TimeOnly(9, 0), new TimeOnly(19, 0), false),
                new Empresa(3,"11.222.333/0001-55", "Clínica Corpo & Alma", "MG", "Belo Horizonte", "Savassi", "Rua da Paz", 789, new TimeOnly(10, 0), new TimeOnly(20, 0), true
                ),
            });
        }
    }
}
