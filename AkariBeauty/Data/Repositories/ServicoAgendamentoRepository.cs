using System;
using System.Data.Entity;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Repositories;

public class ServicoAgendamentoRepository : IServicoAgendamentoRepository
{

    private readonly AppDbContext _context; 
    private readonly DbSet<Agendamento> _dbSet;

    public async Task Add(int servicoId, int agendamentoId)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(int servicoId, int agendamentoId)
    {
        throw new NotImplementedException();
    }
}
