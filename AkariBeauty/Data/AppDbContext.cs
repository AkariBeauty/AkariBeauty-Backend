using AkariBeauty.Data.Builders;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;


namespace AkariBeauty.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Servico> Servicos { get; set; }
		public DbSet<Agendamento> Agendamentos { get; set; }
		public DbSet<Cliente> Clientes { get; set; }
		public DbSet<Empresa> Empresas { get; set; }
		public DbSet<Funcionario> Funcionarios { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            ServicoBuilder.Build(modelBuilder);
			AgendamentoBuilder.Build(modelBuilder);
			EmpresaBuilder.Build(modelBuilder);
			ClienteBuilder.Build(modelBuilder);
			FuncionarioBuilder.Build(modelBuilder);
			



		}
	}
}
