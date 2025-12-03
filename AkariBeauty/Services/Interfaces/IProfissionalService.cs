using System;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Dtos.Profissionais;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;

namespace AkariBeauty.Services.Interfaces;

public interface IProfissionalService : IGenericoService<Profissional, ProfissionalDTO>, IGenericLogin
{
    Task AddServico(ProfissionalServico request);
    Task RemoveServico(int idProfissional, int idServico);
    Task <IEnumerable<ServicoDTO>> GetAllSevicos(int idProfissional);
    Task<IEnumerable<ProfissionalComServicosDTO>> GetByServicoId(int servicoId);
    Task<ProfissionalDashboardDTO> GetDashboardAsync(int profissionalId);
    Task<ProfissionalAgendaDiaDTO> GetAgendaDiaAsync(int profissionalId, DateOnly data);
    Task<IEnumerable<ProfissionalAgendaDiaDTO>> GetAgendaSemanaAsync(int profissionalId, DateOnly inicioSemana);
    Task<ProfissionalAgendamentoDetalheDTO> GetAgendamentoDetalheAsync(int profissionalId, int agendamentoId);
    Task AtualizarStatusAgendamentoAsync(int profissionalId, int agendamentoId, AtualizarStatusAgendamentoDTO request);
    Task<ProfissionalPerfilDTO> GetPerfilAsync(int profissionalId);
    Task AtualizarPerfilAsync(int profissionalId, AtualizarProfissionalPerfilDTO dto);
}
