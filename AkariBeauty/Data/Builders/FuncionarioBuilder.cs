using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Builders
{
    public class FuncionarioBuilder
    {
            public static void Build(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Funcionario>().HasKey(f => f.Id);

                modelBuilder.Entity<Funcionario>().Property(f => f.Nome)
                    .IsRequired().HasMaxLength(100);

                modelBuilder.Entity<Funcionario>().Property(f => f.Cpf)
                    .IsRequired().HasMaxLength(14);

                modelBuilder.Entity<Funcionario>().Property(f => f.Salario)
                    .IsRequired();

                modelBuilder.Entity<Funcionario>().Property(f => f.Login)
                    .IsRequired().HasMaxLength(50);

                modelBuilder.Entity<Funcionario>().Property(f => f.Senha)
                    .IsRequired().HasMaxLength(50);

                modelBuilder.Entity<Funcionario>().HasData(new List<Funcionario>
{
                    new Funcionario(1, "Julia Souza", "123.456.789-00", 3000.00f, "julia.souza", "senha123"),
                    new Funcionario(2, "Marina Rocha", "987.654.321-00", 2800.00f, "marina.rocha", "senha123"),
                    new Funcionario(3, "Patrícia Lima", "321.654.987-00", 3200.00f, "patricia.lima", "senha123")
                });

        }
    }
}
