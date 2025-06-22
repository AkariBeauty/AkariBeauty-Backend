using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Builders
{
    public class ClienteBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasKey(c => c.Id);

            modelBuilder.Entity<Cliente>().Property(c => c.Nome)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Cliente>().Property(c => c.Cpf)
                .IsRequired().HasMaxLength(14);

            modelBuilder.Entity<Cliente>().Property(c => c.Uf)
                .IsRequired().HasMaxLength(2);

            modelBuilder.Entity<Cliente>().Property(c => c.Cidade)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Cliente>().Property(c => c.Bairro)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Cliente>().Property(c => c.Rua)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Cliente>().Property(c => c.Numero)
                .IsRequired();

            modelBuilder.Entity<Cliente>().Property(c => c.Login)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Cliente>().Property(c => c.Senha)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Cliente>().Property(c => c.Telefone)
                .IsRequired().HasMaxLength(15);

            modelBuilder.Entity<Cliente>()
            .HasData(new List<Cliente>
        {
                new Cliente(1, "Joana Silva", "123.456.789-00", "SP", "São Paulo", "Centro", "Rua da Beleza", 456, "joana", "1234", "(11) 91234-5678"),
                new Cliente(2, "Marcos Oliveira", "987.654.321-00", "RJ", "Rio de Janeiro", "Copacabana", "Avenida das Acácias", 789, "marcos", "abcd", "(21) 98765-4321"),
                new Cliente(3, "Ana Costa", "456.123.789-11", "MG", "Belo Horizonte", "Savassi", "Rua das Flores", 101, "ana.costa", "senha123", "(31) 99876-5432")
            });
        }
    }
}
