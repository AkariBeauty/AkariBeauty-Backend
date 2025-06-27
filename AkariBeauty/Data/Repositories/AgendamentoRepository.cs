using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories
{
    public class AgendamentoRepository : GenericoRepository<Agendamento>, IAgendamentoRepository
    {
        private readonly AppDbContext _context;

        private readonly DbSet<Agendamento> _dbSet;

        public AgendamentoRepository(AppDbContext context) : base(context)
        {
            this._context = context;
            this._dbSet = _context.Set<Agendamento>();
        }

        public async Task<IEnumerable<Agendamento>> GetAgendamentos(int idusuario)
        {
            // A consulta une as tabelas exatamente como no seu SQL
            var query = from agendamento in _context.Agendamentos
                        join profissional in _context.Profissionais on agendamento.ProfissionalId equals profissional.Id
                        join empresa in _context.Empresas on profissional.EmpresaId equals empresa.Id
                        join usuario in _context.Usuarios on empresa.Id equals usuario.EmpresaId
                        where usuario.Id == idusuario
                        select agendamento;

            // O .Distinct() remove as duplicatas criadas pelo JOIN com a tabela de usuários
            // E o ToListAsync() executa a consulta no banco de dados.
            return await query.Distinct().OrderBy(a => a.Data).ToListAsync();
        } 
    }
}
