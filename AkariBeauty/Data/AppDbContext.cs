using AkariBeauty.Data.Builders;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;


namespace AkariBeauty.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Servico> Servicos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ServicoBuilder.Build(modelBuilder);


        }
    }
}
