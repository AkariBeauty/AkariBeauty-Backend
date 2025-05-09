using System;
using Microsoft.EntityFrameworkCore;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Repositories;

public class ServicoAgendamentoRepository : IServicoAgendamentoRepository
{

    private readonly AppDbContext _context; 
    private readonly DbSet<Agendamento> _dbSet;

    public ServicoAgendamentoRepository(AppDbContext context)
    {
        this._context = context;
        this._dbSet = _context.Set<Agendamento>();
    }

    public async Task Add(int agendamentoId, int servicoId)
    {
        var agendamento = await _dbSet
            .Include(a => a.Servicos)
            .FirstOrDefaultAsync(a => a.Id == agendamentoId)
            ?? throw new ArgumentException("Agendamento não encontrado.");

        var servico = await _context.Servicos.FindAsync(servicoId)
            ?? throw new ArgumentException("Serviço não encontrado.");
            

        if (agendamento.Servicos.Any(s => s.Id == servico.Id))
            throw new ArgumentException("Serviço ja adicionado.");

        agendamento.Servicos.Add(servico);
        await SaveChanges();
    }

    public async Task Delete(int agendamentoId, int servicoId)
    {
        var agendamento = await _dbSet
            .Include(f => f.Servicos)
            .FirstOrDefaultAsync(f => f.Id == agendamentoId) ?? throw new ArgumentException("Agendamento nao encontrado");
            
        var servico = _context.Servicos.FirstOrDefault(s => s.Id == servicoId) ?? throw new ArgumentException("Servico nao encontrado");

        if (!agendamento.Servicos.Any(s => s.Id == servicoId))
            throw new ArgumentException("Servico e agendamento não vinculados.");

        agendamento.Servicos.Remove(servico);
        await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
