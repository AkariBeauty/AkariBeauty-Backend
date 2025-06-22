using AkariBeauty.Data.Builders;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
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
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<CategoriaServico> CategoriasServicos { get; set; }
		public DbSet<Profissional> Profissionais { get; set; }
		public DbSet<ProfissionalServico> ProfissionaisServicos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			ServicoBuilder.Build(modelBuilder);
			AgendamentoBuilder.Build(modelBuilder);
			EmpresaBuilder.Build(modelBuilder);
			ClienteBuilder.Build(modelBuilder);
			UsuarioBuilder.Build(modelBuilder);
			ServicoAgendamentoBuilder.Build(modelBuilder);
			CategoriaServicoBuilder.Build(modelBuilder);
			ProfissionalBuilder.Build(modelBuilder);
			ProfissionalServicoBuilder.Build(modelBuilder);

		}
	}
}
