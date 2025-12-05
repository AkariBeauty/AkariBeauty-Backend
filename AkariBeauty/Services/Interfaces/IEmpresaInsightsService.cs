using System;
using System.Threading.Tasks;
using AkariBeauty.Objects.Dtos.Empresa;

namespace AkariBeauty.Services.Interfaces
{
    public interface IEmpresaInsightsService
    {
        Task<EmpresaDashboardResponseDto> GetDashboardAsync(int empresaId);
        Task<EmpresaProfessionalsResponseDto> GetProfessionalsAsync(int empresaId);
        Task<EmpresaServicesResponseDto> GetServicesAsync(int empresaId);
        Task<EmpresaAgendaResponseDto> GetAgendaAsync(int empresaId, DateOnly? inicio = null, DateOnly? fim = null);
        Task<EmpresaClientsResponseDto> GetClientsAsync(int empresaId);
        Task<EmpresaFinanceResponseDto> GetFinanceAsync(int empresaId);
        Task<EmpresaSettingsResponseDto> GetSettingsAsync(int empresaId);
        Task<EmpresaCommunicationResponseDto> GetCommunicationAsync(int empresaId);
        Task<EmpresaAuditResponseDto> GetAuditAsync(int empresaId);
    }
}
