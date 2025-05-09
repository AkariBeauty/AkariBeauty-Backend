using System;

namespace AkariBeauty.Services.Interfaces;

public interface IServicoAgendamentoService
{
    Task VincularServicoAoAgendamento(int agendamentoId, int servicoId); 
    Task DesvincularServicoDoAgendamento(int agendamentoId, int servicoId);
}
