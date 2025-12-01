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

            modelBuilder.Entity<Servico>().Property(s => s.EmpresaId)
                .IsRequired();

            modelBuilder.Entity<Servico>().Property(s => s.CategoriaServicoId)
                .IsRequired();

            modelBuilder.Entity<Servico>()
                .HasOne(e => e.Empresa)
                .WithMany(s => s.Servicos)
                .HasForeignKey(s => s.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Servico>()
                .HasOne(cs => cs.CategoriaServico)
                .WithMany(s => s.Servicos)
                .HasForeignKey(cs => cs.CategoriaServicoId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Servico>().HasData(new List<Servico>
            {
                new Servico(1, "Corte Feminino Premium", "Corte personalizado com finalização", 120.00f, 1, 5),
                new Servico(2, "Manicure Express", "Esmaltação e cuidados rápidos", 45.00f, 1, 4),
                new Servico(3, "Massagem Relaxante", "Sessão de 60 minutos com óleos essenciais", 160.00f, 6, 7),
                new Servico(4, "Coloração Completa", "Coloração profissional com tratamento", 280.00f, 4, 5),
                new Servico(5, "Limpeza de Pele Clássica", "Protocolo completo para revitalização facial", 150.00f, 9, 1),
                new Servico(6, "Barba Premium", "Design, navalha e hidratação", 80.00f, 8, 9),
                new Servico(7, "Spa dos Pés", "Esfoliação e hidratação profunda", 95.00f, 6, 4),
                new Servico(8, "Depilação a Cera Corporal", "Pernas e braços completos", 130.00f, 5, 3),
                new Servico(9, "Design de Sobrancelhas", "Medição, correção e aplicação de henna", 70.00f, 9, 8),
                new Servico(10, "Day Spa Corporal", "Esfoliação, massagem e máscara nutritiva", 320.00f, 7, 2)
            });
        }
    }
}