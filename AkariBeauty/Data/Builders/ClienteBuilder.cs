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
                new Cliente(3, "Ana Costa", "456.123.789-11", "MG", "Belo Horizonte", "Savassi", "Rua das Flores", 101, "ana.costa", "senha123", "(31) 99876-5432"),
                new Cliente(4, "Beatriz Ramos", "741.258.963-00", "SP", "Santos", "Gonzaga", "Rua Floriano Peixoto", 210, "bia.ramos", "pass123", "(13) 99741-3698"),
                new Cliente(5, "Daniel Moreira", "852.369.741-11", "DF", "Brasília", "Asa Sul", "SQN 202 Bloco B", 12, "daniel.moreira", "pass123", "(61) 98123-4567"),
                new Cliente(6, "Fernanda Dias", "963.258.147-22", "PR", "Curitiba", "Água Verde", "Rua das Sequoias", 908, "fernanda.dias", "pass123", "(41) 99988-7766"),
                new Cliente(7, "Gustavo Lima", "159.357.258-33", "RS", "Porto Alegre", "Cidade Baixa", "Rua João Alfredo", 112, "gustavo.lima", "pass123", "(51) 98444-3322"),
                new Cliente(8, "Helena Prado", "258.147.369-44", "PE", "Recife", "Boa Viagem", "Rua dos Navegantes", 700, "helena.prado", "pass123", "(81) 99876-5555"),
                new Cliente(9, "Igor Martins", "357.951.456-55", "BA", "Salvador", "Barra", "Avenida Sete", 455, "igor.martins", "pass123", "(71) 99777-2211"),
                new Cliente(10, "Juliana Pires", "456.852.147-66", "SC", "Florianópolis", "Trindade", "Rua Lauro Linhares", 1480, "juliana.pires", "pass123", "(48) 99666-7788")
            });
        }
    }
}
