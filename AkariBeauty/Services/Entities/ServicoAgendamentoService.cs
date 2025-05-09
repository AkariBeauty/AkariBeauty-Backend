using System;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Services.Interfaces;

namespace AkariBeauty.Services.Entities;

public class ServicoAgendamentoService : IServicoAgendamentoService
{
    private readonly IServicoAgendamentoRepository _repository;

    public ServicoAgendamentoService(IServicoAgendamentoRepository repository)
    {
        this._repository = repository;
    }
    public Task DesvincularServicoDoAgendamento(int agendamentoId, int servicoId)
    {
        return _repository.Delete(agendamentoId, servicoId);
    }

    public Task VincularServicoAoAgendamento(int agendamentoId, int servicoId)
    {
        return _repository.Add(agendamentoId, servicoId);
    }
}
