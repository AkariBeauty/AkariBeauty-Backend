using Microsoft.EntityFrameworkCore;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Builders
{
    public class ServicoBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Servico>().HasKey(s => s.Id);

            modelBuilder.Entity<Servico>().Property(s => s.ServicoPrestado)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Servico>().Property(s => s.Descricao)
                .IsRequired().HasMaxLength(255);

            modelBuilder.Entity<Servico>().Property(s => s.ValorBase)
                .IsRequired();

            modelBuilder.Entity<Servico>().HasData(new List<Servico>
            {
                new Servico(1, "Corte de Cabelo", "Corte masculino e feminino", 50.00f),
                new Servico(2, "Manicure", "Manicure completa", 30.00f),
                new Servico(3, "Massagem Relaxante", "Sessão de massagem relaxante", 100.00f),
            });
        }
    }
}