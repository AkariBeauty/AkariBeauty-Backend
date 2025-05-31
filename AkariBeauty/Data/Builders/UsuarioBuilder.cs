using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Builders
{
    public class UsuarioBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasKey(f => f.Id);

            modelBuilder.Entity<Usuario>().Property(f => f.Nome)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Usuario>().Property(f => f.Cpf)
                .IsRequired().HasMaxLength(14);

            modelBuilder.Entity<Usuario>().Property(f => f.Salario)
                .IsRequired();

            modelBuilder.Entity<Usuario>().Property(f => f.Login)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Usuario>().Property(f => f.Senha)
                .IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Usuario>().Property(f => f.EmpresaId)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasOne(e => e.Empresa)
                .WithMany(f => f.Usuarios)
                .HasForeignKey(f => f.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>().HasData(new List<Usuario>
{
                    new Usuario(1, "Julia Souza", "123.456.789-00", 3000.00f, "julia.souza", "senha123", 1),
                    new Usuario(2, "Marina Rocha", "987.654.321-00", 2800.00f, "marina.rocha", "senha123", 1),
                    new Usuario(3, "Patrícia Lima", "321.654.987-00", 3200.00f, "patricia.lima", "senha123", 1)
                });

        }
    }
}
