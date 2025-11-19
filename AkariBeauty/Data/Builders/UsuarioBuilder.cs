using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Enums;

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

            modelBuilder.Entity<Usuario>().Property(f => f.TipoUsuario)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasOne(e => e.Empresa)
                .WithMany(f => f.Usuarios)
                .HasForeignKey(f => f.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>().HasData(new List<Usuario>
{
                    new Usuario(1, "Julia Souza", "123.456.789-00", 3000.00f, "julia.souza", "senha123", TipoUsuario.ADMIN, 1),
                    new Usuario(2, "Marina Rocha", "987.654.321-00", 2800.00f, "marina.rocha", "senha123",TipoUsuario.RECEPCIONISTA, 1),
                    new Usuario(3, "Patrícia Lima", "321.654.987-00", 3200.00f, "patricia.lima", "senha123",TipoUsuario.RECEPCIONISTA, 1),
                    new Usuario(4, "Rafael Gomes", "741.852.963-00", 2900.00f, "rafael.gomes", "senha123", TipoUsuario.ADMIN, 2),
                    new Usuario(5, "Sofia Nunes", "852.963.147-11", 2700.00f, "sofia.nunes", "senha123", TipoUsuario.RECEPCIONISTA, 2),
                    new Usuario(6, "Thiago Peixoto", "963.147.258-22", 3100.00f, "thiago.peixoto", "senha123", TipoUsuario.PROFISSIONAL, 4),
                    new Usuario(7, "Larissa Melo", "159.753.486-33", 3050.00f, "larissa.melo", "senha123", TipoUsuario.PROFISSIONAL, 6),
                    new Usuario(8, "Bruno Vilar", "258.369.741-44", 3300.00f, "bruno.vilar", "senha123", TipoUsuario.ADMIN, 7),
                    new Usuario(9, "Carolina Farias", "357.258.963-55", 2750.00f, "carolina.farias", "senha123", TipoUsuario.RECEPCIONISTA, 8),
                    new Usuario(10, "Marcelo Teles", "456.147.258-66", 3150.00f, "marcelo.teles", "senha123", TipoUsuario.PROFISSIONAL, 9)
                });

        }
    }
}
