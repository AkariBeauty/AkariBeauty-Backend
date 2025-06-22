using System;

namespace AkariBeauty.Data.Interfaces;

public interface IServicoAgendamentoRepository
{
    Task Add(int servicoId, int agendamentoId);
    Task Delete(int servicoId, int agendamentoId);
}