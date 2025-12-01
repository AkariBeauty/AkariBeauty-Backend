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
                .IsRequired().HasMaxLength(14);

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
                new Empresa(1, "70124156000160", "Akari Beauty Center", "SP", "São Paulo", "Centro", "Rua das Rosas", 123, new TimeOnly(8, 0), new TimeOnly(18, 0), true),
                new Empresa(2, "13703006000177", "Estética Bela Vida", "RJ", "Rio de Janeiro", "Copacabana", "Avenida Atlântica", 456, new TimeOnly(9, 0), new TimeOnly(19, 0), false),
                new Empresa(3, "28333675000171", "Clínica Corpo & Alma", "MG", "Belo Horizonte", "Savassi", "Rua da Paz", 789, new TimeOnly(10, 0), new TimeOnly(20, 0), true),
                new Empresa(4, "45872193000109", "Studio Lux Hair", "SP", "Campinas", "Cambuí", "Rua das Camélias", 215, new TimeOnly(9, 0), new TimeOnly(18, 0), true),
                new Empresa(5, "56983214000180", "Casa do Corte Premium", "RJ", "Niterói", "Icaraí", "Rua Quintino Bocaiúva", 980, new TimeOnly(8, 30), new TimeOnly(19, 30), false),
                new Empresa(6, "67294325000103", "Urban Spa & Relax", "PR", "Curitiba", "Batel", "Alameda Prudente de Moraes", 120, new TimeOnly(10, 0), new TimeOnly(21, 0), true),
                new Empresa(7, "78305436000186", "Ateliê Essência Bela", "RS", "Porto Alegre", "Moinhos de Vento", "Rua Padre Chagas", 77, new TimeOnly(9, 30), new TimeOnly(20, 0), true),
                new Empresa(8, "89416547000186", "Barber Club Elite", "SP", "São Paulo", "Moema", "Avenida Ibirapuera", 1950, new TimeOnly(9, 0), new TimeOnly(22, 0), false),
                new Empresa(9, "90527658000149", "Glow Skin Clinic", "BA", "Salvador", "Pituba", "Rua Ceará", 65, new TimeOnly(8, 0), new TimeOnly(18, 0), true),
                new Empresa(10, "01638769000100", "Zen Estética Integrada", "SC", "Florianópolis", "Centro", "Rua Felipe Schmidt", 1340, new TimeOnly(9, 0), new TimeOnly(19, 0), true)
            });
        }
    }
}
