using System.Collections.Generic;

namespace AkariBeauty.Objects.Dtos.Empresa
{
    public class EmpresaKpiDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public double Variation { get; set; }
    }

    public class EmpresaWeeklyTrendDto
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    public class EmpresaRankingItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public double Delta { get; set; }
    }

    public class EmpresaAlertDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }

    public class EmpresaDashboardResponseDto
    {
        public IReadOnlyCollection<EmpresaKpiDto> Kpis { get; set; } = new List<EmpresaKpiDto>();
        public IReadOnlyCollection<EmpresaWeeklyTrendDto> WeeklyTrend { get; set; } = new List<EmpresaWeeklyTrendDto>();
        public IReadOnlyCollection<EmpresaRankingItemDto> Services { get; set; } = new List<EmpresaRankingItemDto>();
        public IReadOnlyCollection<EmpresaRankingItemDto> Professionals { get; set; } = new List<EmpresaRankingItemDto>();
        public IReadOnlyCollection<EmpresaAlertDto> Alerts { get; set; } = new List<EmpresaAlertDto>();
    }

    public class EmpresaProfessionalHighlightDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public double Trend { get; set; }
    }

    public class EmpresaProfessionalSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Specialties { get; set; } = new List<string>();
        public int PresenceRate { get; set; }
        public int AverageDaily { get; set; }
        public string NextShift { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Tags { get; set; } = new List<string>();
    }

    public class EmpresaProfessionalFiltersDto
    {
        public IReadOnlyCollection<string> Statuses { get; set; } = new List<string>();
        public IReadOnlyCollection<string> Services { get; set; } = new List<string>();
    }

    public class EmpresaProfessionalsResponseDto
    {
        public IReadOnlyCollection<EmpresaProfessionalHighlightDto> Highlights { get; set; } = new List<EmpresaProfessionalHighlightDto>();
        public IReadOnlyCollection<EmpresaProfessionalSummaryDto> Professionals { get; set; } = new List<EmpresaProfessionalSummaryDto>();
        public EmpresaProfessionalFiltersDto Filters { get; set; } = new EmpresaProfessionalFiltersDto();
    }

    public class EmpresaServiceCatalogItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }

    public class EmpresaServiceHistoryEntryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Changes { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
    }

    public class EmpresaServicesResponseDto
    {
        public IReadOnlyCollection<EmpresaServiceCatalogItemDto> Catalog { get; set; } = new List<EmpresaServiceCatalogItemDto>();
        public IReadOnlyCollection<EmpresaServiceHistoryEntryDto> History { get; set; } = new List<EmpresaServiceHistoryEntryDto>();
    }

    public class EmpresaAgendaSummaryDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }

    public class EmpresaAgendaSlotDto
    {
        public int Id { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public string Professional { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class EmpresaAgendaFiltersDto
    {
        public IReadOnlyCollection<string> Professionals { get; set; } = new List<string>();
        public IReadOnlyCollection<string> Services { get; set; } = new List<string>();
    }

    public class EmpresaAgendaResponseDto
    {
        public IReadOnlyCollection<EmpresaAgendaSummaryDto> Summary { get; set; } = new List<EmpresaAgendaSummaryDto>();
        public IReadOnlyCollection<EmpresaAgendaSlotDto> Slots { get; set; } = new List<EmpresaAgendaSlotDto>();
        public EmpresaAgendaFiltersDto Filters { get; set; } = new EmpresaAgendaFiltersDto();
    }

    public class EmpresaClientMetricDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public double Delta { get; set; }
    }

    public class EmpresaClientRecordDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Segment { get; set; } = string.Empty;
        public int Visits { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Retention { get; set; } = string.Empty;
        public string LifetimeValue { get; set; } = string.Empty;
        public string LastVisit { get; set; } = string.Empty;
    }

    public class EmpresaClientsResponseDto
    {
        public IReadOnlyCollection<EmpresaClientMetricDto> Metrics { get; set; } = new List<EmpresaClientMetricDto>();
        public IReadOnlyCollection<EmpresaClientRecordDto> Clients { get; set; } = new List<EmpresaClientRecordDto>();
    }

    public class EmpresaFinanceKpiDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public double Variation { get; set; }
    }

    public class EmpresaCashFlowEntryDto
    {
        public string Period { get; set; } = string.Empty;
        public double Entradas { get; set; }
        public double Saidas { get; set; }
    }

    public class EmpresaCommissionRowDto
    {
        public string Professional { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class EmpresaFinanceResponseDto
    {
        public IReadOnlyCollection<EmpresaFinanceKpiDto> Kpis { get; set; } = new List<EmpresaFinanceKpiDto>();
        public IReadOnlyCollection<EmpresaCashFlowEntryDto> CashFlow { get; set; } = new List<EmpresaCashFlowEntryDto>();
        public IReadOnlyCollection<EmpresaCommissionRowDto> Commissions { get; set; } = new List<EmpresaCommissionRowDto>();
    }

    public class EmpresaSettingsLegalDto
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Ie { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
    }

    public class EmpresaSettingsContactDto
    {
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Whatsapp { get; set; } = string.Empty;
    }

    public class EmpresaSettingsAddressDto
    {
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
    }

    public class EmpresaSettingsHoursDto
    {
        public string SegundaSexta { get; set; } = string.Empty;
        public string Sabado { get; set; } = string.Empty;
        public string Domingo { get; set; } = string.Empty;
    }

    public class EmpresaSettingsNotificationsDto
    {
        public bool EmailFinanceiro { get; set; }
        public bool SmsClientes { get; set; }
        public bool PushEquipe { get; set; }
    }

    public class EmpresaSettingsBrandDto
    {
        public string LogoUrl { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
    }

    public class EmpresaSettingsResponseDto
    {
        public EmpresaSettingsLegalDto Legal { get; set; } = new EmpresaSettingsLegalDto();
        public EmpresaSettingsContactDto Contact { get; set; } = new EmpresaSettingsContactDto();
        public EmpresaSettingsAddressDto Address { get; set; } = new EmpresaSettingsAddressDto();
        public EmpresaSettingsHoursDto Hours { get; set; } = new EmpresaSettingsHoursDto();
        public EmpresaSettingsNotificationsDto Notifications { get; set; } = new EmpresaSettingsNotificationsDto();
        public EmpresaSettingsBrandDto Brand { get; set; } = new EmpresaSettingsBrandDto();
    }

    public class EmpresaCommunicationTemplateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string LastSend { get; set; } = string.Empty;
        public double OpenRate { get; set; }
    }

    public class EmpresaCommunicationStatsDto
    {
        public int SentThisWeek { get; set; }
        public double DeliveryRate { get; set; }
        public int ActiveCampaigns { get; set; }
    }

    public class EmpresaCommunicationResponseDto
    {
        public EmpresaCommunicationStatsDto Stats { get; set; } = new EmpresaCommunicationStatsDto();
        public IReadOnlyCollection<EmpresaCommunicationTemplateDto> Templates { get; set; } = new List<EmpresaCommunicationTemplateDto>();
    }

    public class EmpresaLogEntryDto
    {
        public int Id { get; set; }
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Actor { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
    }

    public class EmpresaAuditFiltersDto
    {
        public IReadOnlyCollection<string> Modules { get; set; } = new List<string>();
        public IReadOnlyCollection<string> Actors { get; set; } = new List<string>();
    }

    public class EmpresaAuditResponseDto
    {
        public IReadOnlyCollection<EmpresaLogEntryDto> Logs { get; set; } = new List<EmpresaLogEntryDto>();
        public EmpresaAuditFiltersDto Filters { get; set; } = new EmpresaAuditFiltersDto();
    }
}
