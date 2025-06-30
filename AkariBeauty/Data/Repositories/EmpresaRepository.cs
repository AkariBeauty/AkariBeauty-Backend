using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Services.Types;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories
{
    public class EmpresaRepository : GenericoRepository<Empresa>, IEmpresaRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Empresa> _dbSet;
        public EmpresaRepository(AppDbContext context) : base(context)
        {
            this._context = context;
            this._dbSet = _context.Set<Empresa>();
        }

        public async Task<Empresa> FindByCnpj(string cnpj)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Cnpj == cnpj);
        }

        public async Task<IEnumerable<Profissional>> GetAgendamentosPorProfissionalEData(int idusuario, DateOnly data)
        {
            var agendamentos = await _context.Profissionais
            .Include(a => a.Agendamentos
                .Where(
                    p =>
                        (
                            p.StatusAgendamento == StatusAgendamento.CONFIRMADO
                            || p.StatusAgendamento == StatusAgendamento.PENDENTE
                            || p.StatusAgendamento == StatusAgendamento.COBRADO
                            || p.StatusAgendamento == StatusAgendamento.REALIZADO
                        )
                    &&
                        p.Data == data
                )
            )
            .Include(a => a.ProfissionalServicos)
            .Where(
                    p => p.Empresa.Usuarios.Any(u => u.Id == idusuario)
                ).ToListAsync();

            return agendamentos;
        }

        public async Task<IEnumerable<Cliente>> GetNovosClientes(DateOnly only, int idusuario)
        {
            var cliente = await _context.Clientes
            .Where(c =>
                c.Agendamentos.Where(a => a.StatusAgendamento == StatusAgendamento.REALIZADO)
                    .Select(a => a.ClienteId)
                    .Contains(c.Id)
                &&
                !c.Agendamentos
                        .Where(a => a.Data < only
                            && a.Profissional != null
                            && a.Profissional.Empresa != null
                            && a.Profissional.Empresa.Usuarios != null
                            && a.Profissional.Empresa.Usuarios.Any(u => u.Id == idusuario)
                        )
                        .Select(a => a.ClienteId)
                        .Distinct()
                        .Contains(c.Id)
            ).ToListAsync();

            return cliente;

        }
    }
}
