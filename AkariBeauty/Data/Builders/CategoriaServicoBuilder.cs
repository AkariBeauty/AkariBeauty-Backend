using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Builders;

public class CategoriaServicoBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaServico>(m =>
        {
            m.HasKey(cs => cs.Id);

            m.Property(cs => cs.Categoria)
                .IsRequired()
                .HasMaxLength(100);

            m.Property(cs => cs.Descricao)
                .IsRequired()
                .HasMaxLength(255);

            m.HasData(new List<CategoriaServico>
            {
                // Categorias Estéticas Gerais
                new CategoriaServico { Id = 1, Categoria = "Estética Facial", Descricao = "Tratamentos e cuidados para a pele do rosto, incluindo limpezas e hidratações." },
                new CategoriaServico { Id = 2, Categoria = "Estética Corporal", Descricao = "Procedimentos para o corpo, como massagens e tratamentos para celulite e estrias." },
                new CategoriaServico { Id = 3, Categoria = "Depilação", Descricao = "Métodos de remoção de pelos para diversas partes do corpo." },
                new CategoriaServico { Id = 4, Categoria = "Manicure e Pedicure", Descricao = "Cuidados e embelezamento das mãos e pés, incluindo unhas e esmaltação." },
                new CategoriaServico { Id = 5, Categoria = "Cabelo", Descricao = "Serviços completos para o cabelo, desde cortes e colorações até tratamentos." },
                new CategoriaServico { Id = 6, Categoria = "Maquiagem", Descricao = "Aplicação de maquiagem para diversas ocasiões e cursos de automaquiagem." },
                new CategoriaServico { Id = 7, Categoria = "Terapias de Bem-Estar", Descricao = "Massagens terapêuticas e holísticas para relaxamento e equilíbrio." },
                new CategoriaServico { Id = 8, Categoria = "Micropigmentação e Dermopigmentação", Descricao = "Procedimentos estéticos que utilizam pigmentos na pele, como sobrancelhas e lábios." },

                // Categorias com Foco no Público Masculino
                new CategoriaServico { Id = 9, Categoria = "Barbearia e Cuidados com a Barba", Descricao = "Cortes de cabelo masculinos, aparo e design de barba, e tratamentos específicos." },
                new CategoriaServico { Id = 10, Categoria = "Estética Facial Masculina", Descricao = "Limpeza de pele, hidratação e tratamentos faciais específicos para a pele masculina." },
                new CategoriaServico { Id = 11, Categoria = "Depilação Masculina", Descricao = "Remoção de pelos corporais e faciais para homens, com foco em conforto e eficiência." },
                new CategoriaServico { Id = 12, Categoria = "Manicure e Pedicure Masculina", Descricao = "Cuidados essenciais para unhas e pés masculinos, visando higiene e boa aparência." },
                new CategoriaServico { Id = 13, Categoria = "Tratamentos Capilares Masculinos", Descricao = "Soluções para queda de cabelo, oleosidade e hidratação do couro cabeludo e fios masculinos." },
            });

        });
    }
}
