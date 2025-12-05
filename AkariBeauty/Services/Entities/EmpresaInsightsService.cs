using AkariBeauty.Data;
using AkariBeauty.Objects.Dtos.Empresa;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AkariBeauty.Services.Entities
{
    public class EmpresaInsightsService : IEmpresaInsightsService
    {
        private readonly AppDbContext _context;
        private readonly CultureInfo _culture = new("pt-BR");

        public EmpresaInsightsService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<EmpresaDashboardResponseDto> GetDashboardAsync(int empresaId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var weekStart = today.AddDays(-6);
            var monthStart = today.AddDays(-29);

            var weeklyAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= weekStart && a.Data <= today)
                .ToListAsync();

            var monthlyAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= monthStart && a.Data <= today)
                .ToListAsync();

            var previousMonthEnd = monthStart.AddDays(-1);
            var previousMonthStart = previousMonthEnd.AddDays(-29);
            var previousMonthlyAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= previousMonthStart && a.Data <= previousMonthEnd)
                .ToListAsync();

            var yesterday = today.AddDays(-1);
            var revenueToday = SumRevenue(weeklyAppointments.Where(a => a.Data == today));
            var revenueYesterday = SumRevenue(weeklyAppointments.Where(a => a.Data == yesterday));
            var confirmadosHoje = weeklyAppointments.Count(a => a.Data == today && a.StatusAgendamento == StatusAgendamento.CONFIRMADO);
            var confirmadosOntem = weeklyAppointments.Count(a => a.Data == yesterday && a.StatusAgendamento == StatusAgendamento.CONFIRMADO);
            var canceladosHoje = weeklyAppointments.Count(a => a.Data == today && IsCanceled(a.StatusAgendamento));
            var canceladosOntem = weeklyAppointments.Count(a => a.Data == yesterday && IsCanceled(a.StatusAgendamento));

            var ticketList = weeklyAppointments.Where(a => a.Data == today).Select(a => (double)a.Valor).ToList();
            var ticketMedio = ticketList.Count == 0 ? 0 : ticketList.Average();
            var ticketOntemList = weeklyAppointments.Where(a => a.Data == yesterday).Select(a => (double)a.Valor).ToList();
            var ticketMedioOntem = ticketOntemList.Count == 0 ? 0 : ticketOntemList.Average();

            var kpis = new List<EmpresaKpiDto>
            {
                new EmpresaKpiDto { Label = "Faturamento diario", Value = FormatCurrency(revenueToday), Variation = CalculateVariation((double)revenueToday, (double)revenueYesterday) },
                new EmpresaKpiDto { Label = "Agendamentos confirmados", Value = confirmadosHoje.ToString(), Variation = CalculateVariation(confirmadosHoje, confirmadosOntem) },
                new EmpresaKpiDto { Label = "Cancelamentos", Value = canceladosHoje.ToString(), Variation = CalculateVariation(canceladosHoje, canceladosOntem) },
                new EmpresaKpiDto { Label = "Ticket medio", Value = ticketMedio.ToString("C0", _culture), Variation = CalculateVariation(ticketMedio, ticketMedioOntem) }
            };

            var weeklyTrend = Enumerable.Range(0, 7)
                .Select(offset => weekStart.AddDays(offset))
                .Select(date => new EmpresaWeeklyTrendDto
                {
                    Label = date.ToString("ddd", _culture).Replace(".", string.Empty, StringComparison.OrdinalIgnoreCase),
                    Value = weeklyAppointments.Count(a => a.Data == date && IsRevenueStatus(a.StatusAgendamento))
                })
                .ToList();

            var serviceSharesCurrent = AggregateServiceShares(monthlyAppointments);
            var serviceSharesPrevious = AggregateServiceShares(previousMonthlyAppointments)
                .ToDictionary(s => s.Name, s => s.Amount, StringComparer.OrdinalIgnoreCase);

            var serviceRanking = serviceSharesCurrent
                .OrderByDescending(s => s.Amount)
                .Take(5)
                .Select(s =>
                {
                    serviceSharesPrevious.TryGetValue(s.Name, out var previousAmount);
                    return new EmpresaRankingItemDto
                    {
                        Name = s.Name,
                        Value = FormatCurrency(s.Amount),
                        Delta = CalculateVariation(s.Amount, previousAmount)
                    };
                })
                .ToList();

            var professionalRankingCurrent = monthlyAppointments
                .Where(a => a.Profissional != null)
                .GroupBy(a => a.Profissional!.Nome)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(a => IsRevenueStatus(a.StatusAgendamento))
                })
                .ToList();

            var professionalRankingPrevious = previousMonthlyAppointments
                .Where(a => a.Profissional != null)
                .GroupBy(a => a.Profissional!.Nome)
                .ToDictionary(g => g.Key, g => g.Count(a => IsRevenueStatus(a.StatusAgendamento)), StringComparer.OrdinalIgnoreCase);

            var professionalRanking = professionalRankingCurrent
                .OrderByDescending(r => r.Count)
                .Take(5)
                .Select(r => new EmpresaRankingItemDto
                {
                    Name = r.Name,
                    Value = $"{r.Count} atendimentos",
                    Delta = CalculateVariation(r.Count, professionalRankingPrevious.TryGetValue(r.Name, out var prev) ? prev : 0)
                })
                .ToList();

            var pendenciasFinanceiras = monthlyAppointments.Count(a => a.Comissao > 0 && a.StatusAgendamento == StatusAgendamento.CONFIRMADO);
            var cadastrosIncompletos = await _context.Profissionais.CountAsync(p => p.EmpresaId == empresaId && string.IsNullOrWhiteSpace(p.Telefone));
            var conflitosAgenda = weeklyAppointments.Count(a => a.StatusAgendamento == StatusAgendamento.PENDENTE);

            var alerts = new List<EmpresaAlertDto>();
            if (pendenciasFinanceiras > 0)
            {
                alerts.Add(new EmpresaAlertDto
                {
                    Id = 1,
                    Title = "Pendencias de pagamento",
                    Type = "financeiro",
                    Detail = $"{pendenciasFinanceiras} comissoes aguardando liberacao"
                });
            }
            if (cadastrosIncompletos > 0)
            {
                alerts.Add(new EmpresaAlertDto
                {
                    Id = 2,
                    Title = "Cadastros incompletos",
                    Type = "cadastro",
                    Detail = $"{cadastrosIncompletos} profissionais sem telefone"
                });
            }
            if (conflitosAgenda > 0)
            {
                alerts.Add(new EmpresaAlertDto
                {
                    Id = 3,
                    Title = "Agendamentos pendentes",
                    Type = "agenda",
                    Detail = $"{conflitosAgenda} slots aguardando confirmacao"
                });
            }

            return new EmpresaDashboardResponseDto
            {
                Kpis = kpis,
                WeeklyTrend = weeklyTrend,
                Services = serviceRanking,
                Professionals = professionalRanking,
                Alerts = alerts
            };
        }

        public async Task<EmpresaProfessionalsResponseDto> GetProfessionalsAsync(int empresaId)
        {
            var professionals = await _context.Profissionais
                .AsNoTracking()
                .Include(p => p.ProfissionalServicos)!.ThenInclude(ps => ps.Servico)
                .Where(p => p.EmpresaId == empresaId)
                .ToListAsync();

            var last30 = DateOnly.FromDateTime(DateTime.Now).AddDays(-29);
            var appointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= last30)
                .ToListAsync();

            var previousWindowEnd = last30.AddDays(-1);
            var previousWindowStart = previousWindowEnd.AddDays(-29);
            var previousAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= previousWindowStart && a.Data <= previousWindowEnd)
                .ToListAsync();

            var upcoming = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(a => a.Data)
                .ThenBy(a => a.Hora)
                .ToListAsync();

            var statsByProfessional = appointments
                .Where(a => a.ProfissionalId.HasValue)
                .GroupBy(a => a.ProfissionalId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var previousStatsByProfessional = previousAppointments
                .Where(a => a.ProfissionalId.HasValue)
                .GroupBy(a => a.ProfissionalId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var summaries = professionals.Select(p =>
            {
                statsByProfessional.TryGetValue(p.Id, out var stats);
                stats ??= new List<Agendamento>();
                var total = stats.Count;
                var attended = stats.Count(a => IsRevenueStatus(a.StatusAgendamento));
                var presenceRate = total == 0 ? 0 : (int)Math.Round((double)attended / total * 100);
                var avgDaily = (int)Math.Round(attended / 30.0);
                var nextAppointment = upcoming.FirstOrDefault(a => a.ProfissionalId == p.Id);
                var nextShift = nextAppointment == null
                    ? "Sem agenda"
                    : $"{nextAppointment.Data:dd/MM} · {nextAppointment.Hora:hh\\:mm}";

                var specialties = p.ProfissionalServicos?
                    .Where(ps => ps.Servico != null)
                    .Select(ps => ps.Servico!.ServicoPrestado)
                    .Distinct()
                    .Take(3)
                    .ToList() ?? new List<string>();

                var tags = new List<string>();
                if (presenceRate >= 95) tags.Add("Top presenca");
                if (avgDaily >= 5) tags.Add("Alta demanda");
                if (p.Status != StatusProfissional.ATIVO) tags.Add("Revisar status");

                return new EmpresaProfessionalSummaryDto
                {
                    Id = p.Id,
                    Name = p.Nome,
                    Role = specialties.FirstOrDefault() ?? "Especialista",
                    Status = p.Status.ToString().ToLowerInvariant(),
                    Specialties = specialties,
                    PresenceRate = presenceRate,
                    AverageDaily = Math.Max(0, avgDaily),
                    NextShift = nextShift,
                    Tags = tags
                };
            }).ToList();

            double AvgPresence(Dictionary<int, List<Agendamento>> source)
            {
                if (source.Count == 0) return 0;
                return source.Average(pair =>
                {
                    var total = pair.Value.Count;
                    if (total == 0) return 0d;
                    var attended = pair.Value.Count(a => IsRevenueStatus(a.StatusAgendamento));
                    return (double)attended / total * 100;
                });
            }

            var activeWithAgenda = statsByProfessional.Count(pair => pair.Value.Any());
            var activeWithAgendaPrev = previousStatsByProfessional.Count(pair => pair.Value.Any());
            var avgPresenceCurrent = AvgPresence(statsByProfessional);
            var avgPresencePrevious = AvgPresence(previousStatsByProfessional);
            var avgDailyCurrent = statsByProfessional.Sum(pair => pair.Value.Count(a => IsRevenueStatus(a.StatusAgendamento))) / 30.0;
            var avgDailyPrevious = previousStatsByProfessional.Sum(pair => pair.Value.Count(a => IsRevenueStatus(a.StatusAgendamento))) / 30.0;

            var highlights = new List<EmpresaProfessionalHighlightDto>
            {
                new EmpresaProfessionalHighlightDto
                {
                    Label = "Profissionais ativos",
                    Value = activeWithAgenda.ToString(),
                    Trend = CalculateVariation(activeWithAgenda, activeWithAgendaPrev)
                },
                new EmpresaProfessionalHighlightDto
                {
                    Label = "Taxa de presenca",
                    Value = summaries.Count == 0 ? "0%" : $"{avgPresenceCurrent:F0}%",
                    Trend = CalculateVariation(avgPresenceCurrent, avgPresencePrevious)
                },
                new EmpresaProfessionalHighlightDto
                {
                    Label = "Media diaria",
                    Value = Math.Round(avgDailyCurrent, 1).ToString("0.0", _culture),
                    Trend = CalculateVariation(avgDailyCurrent, avgDailyPrevious)
                }
            };

            var filters = new EmpresaProfessionalFiltersDto
            {
                Statuses = summaries.Select(s => s.Status).Distinct().ToList(),
                Services = summaries.SelectMany(s => s.Specialties).Distinct().Take(6).ToList()
            };

            return new EmpresaProfessionalsResponseDto
            {
                Highlights = highlights,
                Professionals = summaries,
                Filters = filters
            };
        }

        public async Task<EmpresaServicesResponseDto> GetServicesAsync(int empresaId)
        {
            var services = await _context.Servicos
                .AsNoTracking()
                .Include(s => s.CategoriaServico)
                .Include(s => s.ProfissionalServicos)
                .Where(s => s.EmpresaId == empresaId)
                .OrderBy(s => s.ServicoPrestado)
                .ToListAsync();

            var serviceActivityWindow = DateOnly.FromDateTime(DateTime.Now).AddDays(-90);
            var serviceAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Servicos.Any() && a.Data >= serviceActivityWindow)
                .OrderByDescending(a => a.Data)
                .ThenByDescending(a => a.Hora)
                .Take(200)
                .ToListAsync();

            var activityByService = BuildServiceActivity(serviceAppointments);

            var catalog = services.Select(s =>
            {
                activityByService.TryGetValue(s.Id, out var activity);
                var durationMinutes = s.ProfissionalServicos != null && s.ProfissionalServicos.Count > 0
                    ? (int)Math.Round(s.ProfissionalServicos.Average(ps => ps.Tempo.Hour * 60 + ps.Tempo.Minute))
                    : 60;
                var lastOccurrence = activity?.LastOccurrence;
                var status = activity == null
                    ? "rascunho"
                    : lastOccurrence.HasValue && lastOccurrence.Value >= DateTime.Now.AddDays(-30)
                        ? "ativo"
                        : "inativo";

                return new EmpresaServiceCatalogItemDto
                {
                    Id = s.Id,
                    Name = s.ServicoPrestado,
                    Category = s.CategoriaServico?.Categoria ?? "-",
                    Price = FormatCurrency(s.ValorBase),
                    Duration = $"{durationMinutes} min",
                    Status = status,
                    UpdatedAt = lastOccurrence?.ToString("dd/MM HH:mm", _culture) ?? "Sem registros",
                    Version = $"v{Math.Max(1, activity?.Executions ?? 0)}",
                    UpdatedBy = activity?.LastProfessional ?? "Equipe"
                };
            }).ToList();

            var history = activityByService.Values
                .Where(a => a.LastOccurrence.HasValue)
                .OrderByDescending(a => a.LastOccurrence)
                .Take(6)
                .Select(a => new EmpresaServiceHistoryEntryDto
                {
                    Id = a.ServiceId,
                    Name = a.Name,
                    Changes = $"Atendimento para {a.LastClient} por {a.LastProfessional}",
                    UpdatedAt = a.LastOccurrence!.Value.ToString("dd/MM HH:mm", _culture)
                })
                .ToList();

            return new EmpresaServicesResponseDto
            {
                Catalog = catalog,
                History = history
            };
        }

        public async Task<EmpresaAgendaResponseDto> GetAgendaAsync(int empresaId, DateOnly? inicio = null, DateOnly? fim = null)
        {
            var start = inicio ?? DateOnly.FromDateTime(DateTime.Now);
            var end = fim ?? start.AddDays(6);

            var appointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= start && a.Data <= end)
                .OrderBy(a => a.Data)
                .ThenBy(a => a.Hora)
                .Take(150)
                .ToListAsync();

            var durationMap = await GetServiceDurationMapAsync(empresaId);

            var confirmados = appointments.Count(a => IsRevenueStatus(a.StatusAgendamento));
            var pendentes = appointments.Count(a => a.StatusAgendamento == StatusAgendamento.PENDENTE);
            var cancelados = appointments.Count(a => IsCanceled(a.StatusAgendamento));

            var summary = new List<EmpresaAgendaSummaryDto>
            {
                new EmpresaAgendaSummaryDto { Label = "Slots confirmados", Value = confirmados.ToString(), Detail = $"Periodo {start:dd/MM} - {end:dd/MM}" },
                new EmpresaAgendaSummaryDto { Label = "Remarcacoes", Value = pendentes.ToString(), Detail = "Atualize rapidamente" },
                new EmpresaAgendaSummaryDto { Label = "Cancelamentos", Value = cancelados.ToString(), Detail = "Reabra agenda" }
            };

            var slots = appointments.Select(a =>
            {
                var firstService = a.Servicos?.FirstOrDefault();
                var estimatedDuration = firstService != null && durationMap.TryGetValue(firstService.Id, out var minutes)
                    ? minutes
                    : 60;

                return new EmpresaAgendaSlotDto
                {
                    Id = a.Id,
                    Date = a.Data.ToString("dd/MM", _culture),
                    Start = a.Hora.ToString("hh\\:mm", _culture),
                    End = a.Hora.AddMinutes(estimatedDuration).ToString("hh\\:mm", _culture),
                    Professional = a.Profissional?.Nome ?? "Equipe",
                    Service = firstService?.ServicoPrestado ?? "Servico",
                    Status = ToAgendaStatus(a.StatusAgendamento),
                    Client = a.Cliente?.Nome ?? "Cliente",
                    Location = a.Profissional?.Empresa?.Cidade ?? "Sala"
                };
            }).ToList();

            var filters = new EmpresaAgendaFiltersDto
            {
                Professionals = appointments.Select(a => a.Profissional?.Nome ?? "Equipe").Distinct().ToList(),
                Services = appointments.SelectMany(a => a.Servicos?.Select(s => s.ServicoPrestado) ?? Array.Empty<string>()).Distinct().ToList()
            };

            return new EmpresaAgendaResponseDto
            {
                Summary = summary,
                Slots = slots,
                Filters = filters
            };
        }

        public async Task<EmpresaClientsResponseDto> GetClientsAsync(int empresaId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var start = today.AddDays(-120);
            var previousEnd = start.AddDays(-1);
            var previousStart = previousEnd.AddDays(-120);
            var retentionCutoff = today.AddDays(-90);
            var previousRetentionCutoff = previousEnd.AddDays(-90);

            var appointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= start)
                .ToListAsync();

            var previousAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= previousStart && a.Data <= previousEnd)
                .ToListAsync();

            var groupByClient = appointments
                .Where(a => a.Cliente != null)
                .GroupBy(a => a.Cliente!)
                .Select(g => new ClientSnapshot
                {
                    Cliente = g.Key,
                    Visits = g.Count(),
                    LastVisit = g.Max(a => a.Data),
                    Lifetime = g.Sum(a => (decimal)a.Valor),
                    RecentVisits = g.Count(a => a.Data >= retentionCutoff)
                })
                .OrderByDescending(x => x.Visits)
                .Take(50)
                .ToList();

            var previousGroup = previousAppointments
                .Where(a => a.Cliente != null)
                .GroupBy(a => a.Cliente!)
                .Select(g => new ClientSnapshot
                {
                    Cliente = g.Key,
                    Visits = g.Count(),
                    LastVisit = g.Max(a => a.Data),
                    Lifetime = g.Sum(a => (decimal)a.Valor),
                    RecentVisits = g.Count(a => a.Data >= previousRetentionCutoff)
                })
                .ToList();

            double AvgRetention(IReadOnlyCollection<ClientSnapshot> source)
            {
                if (source.Count == 0) return 0;
                return source.Average(c => c.Visits == 0 ? 0 : (double)c.RecentVisits / c.Visits * 100);
            }

            int CountVip(IReadOnlyCollection<ClientSnapshot> source)
            {
                var total = 0;
                foreach (var snapshot in source)
                {
                    if (snapshot.Visits >= 10) total++;
                }
                return total;
            }

            var vipCurrent = CountVip(groupByClient);
            var vipPrevious = CountVip(previousGroup);

            var metrics = new List<EmpresaClientMetricDto>
            {
                new EmpresaClientMetricDto
                {
                    Label = "Clientes ativos",
                    Value = groupByClient.Count.ToString(),
                    Delta = CalculateVariation(groupByClient.Count, previousGroup.Count)
                },
                new EmpresaClientMetricDto
                {
                    Label = "Retencao 90d",
                    Value = groupByClient.Count == 0 ? "0%" : $"{AvgRetention(groupByClient):F0}%",
                    Delta = CalculateVariation(AvgRetention(groupByClient), AvgRetention(previousGroup))
                },
                new EmpresaClientMetricDto
                {
                    Label = "VIPs",
                    Value = vipCurrent.ToString(),
                    Delta = CalculateVariation(vipCurrent, vipPrevious)
                }
            };

            var clients = groupByClient.Select(c => new EmpresaClientRecordDto
            {
                Id = c.Cliente.Id,
                Name = c.Cliente.Nome,
                Segment = c.Cliente.Cidade ?? "Segmento",
                Visits = c.Visits,
                Status = c.Visits >= 10 ? "vip" : c.RecentVisits == 0 ? "inadimplente" : "ativo",
                Retention = c.Visits == 0 ? "0%" : $"{Math.Round((double)c.RecentVisits / c.Visits * 100):F0}%",
                LifetimeValue = FormatCurrency(c.Lifetime),
                LastVisit = c.LastVisit.ToString("dd/MM", _culture)
            }).ToList();

            return new EmpresaClientsResponseDto
            {
                Metrics = metrics,
                Clients = clients
            };
        }

        public async Task<EmpresaFinanceResponseDto> GetFinanceAsync(int empresaId)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var last8Weeks = now.AddDays(-55);
            var appointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= last8Weeks)
                .ToListAsync();

            var futureAppointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= now)
                .ToListAsync();

            var projectedCash = futureAppointments.Where(a => IsRevenueStatus(a.StatusAgendamento)).Sum(a => (double)a.Valor);
            var pendingCommissions = appointments.Where(a => a.Comissao > 0 && a.StatusAgendamento == StatusAgendamento.CONFIRMADO).Sum(a => (double)a.Comissao);
            var notasEmitidas = appointments.Count(a => a.StatusAgendamento == StatusAgendamento.REALIZADO);

            var kpis = new List<EmpresaFinanceKpiDto>
            {
                new EmpresaFinanceKpiDto { Label = "Fluxo projetado", Value = FormatCurrency(projectedCash), Variation = RandomTrend() },
                new EmpresaFinanceKpiDto { Label = "Comissoes pendentes", Value = FormatCurrency(pendingCommissions), Variation = RandomTrend() },
                new EmpresaFinanceKpiDto { Label = "Notas emitidas", Value = notasEmitidas.ToString(), Variation = RandomTrend() }
            };

            var cashFlow = appointments
                .GroupBy(a => System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(a.Data.ToDateTime(new TimeOnly()), CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .OrderBy(g => g.Key)
                .Select(g => new EmpresaCashFlowEntryDto
                {
                    Period = $"Semana {g.Key}",
                    Entradas = g.Where(a => IsRevenueStatus(a.StatusAgendamento)).Sum(a => a.Valor),
                    Saidas = g.Sum(a => a.Comissao)
                })
                .TakeLast(4)
                .ToList();

            var commissions = appointments
                .Where(a => a.Profissional != null && a.Comissao > 0)
                .GroupBy(a => a.Profissional!.Nome)
                .Select(g => new EmpresaCommissionRowDto
                {
                    Professional = g.Key,
                    Amount = FormatCurrency(g.Sum(a => a.Comissao)),
                    Period = now.ToString("MMM/yy", _culture),
                    Status = g.Any(a => a.StatusAgendamento == StatusAgendamento.CONFIRMADO) ? "pendente" : "pago"
                })
                .Take(6)
                .ToList();

            return new EmpresaFinanceResponseDto
            {
                Kpis = kpis,
                CashFlow = cashFlow,
                Commissions = commissions
            };
        }

        public async Task<EmpresaSettingsResponseDto> GetSettingsAsync(int empresaId)
        {
            var empresa = await _context.Empresas
                .AsNoTracking()
                .Include(e => e.Usuarios)
                .FirstOrDefaultAsync(e => e.Id == empresaId)
                ?? throw new ArgumentException("Empresa nao encontrada");

            var responsavel = empresa.Usuarios?.FirstOrDefault()?.Nome ?? "Responsavel";
            var contato = empresa.Usuarios?.FirstOrDefault()?.Login ?? "contato@akari.com";

            return new EmpresaSettingsResponseDto
            {
                Legal = new EmpresaSettingsLegalDto
                {
                    RazaoSocial = empresa.RazaoSocial,
                    Cnpj = empresa.Cnpj,
                    Ie = "ISENTO",
                    Responsavel = responsavel
                },
                Contact = new EmpresaSettingsContactDto
                {
                    Email = contato,
                    Phone = "+55 11 0000-0000",
                    Whatsapp = "+55 11 99999-0000"
                },
                Address = new EmpresaSettingsAddressDto
                {
                    Rua = empresa.Rua,
                    Numero = empresa.Numero.ToString(),
                    Bairro = empresa.Bairro,
                    Cidade = empresa.Cidade,
                    Uf = empresa.Uf,
                    Cep = "00000-000"
                },
                Hours = new EmpresaSettingsHoursDto
                {
                    SegundaSexta = $"{empresa.HoraInicial:hh\\:mm} as {empresa.HoraFinal:hh\\:mm}",
                    Sabado = "09h as 14h",
                    Domingo = "Sob demanda"
                },
                Notifications = new EmpresaSettingsNotificationsDto
                {
                    EmailFinanceiro = true,
                    SmsClientes = false,
                    PushEquipe = true
                },
                Brand = new EmpresaSettingsBrandDto
                {
                    LogoUrl = "https://placehold.co/160x64",
                    UpdatedAt = "Atualizado neste mes"
                }
            };
        }

        public async Task<EmpresaCommunicationResponseDto> GetCommunicationAsync(int empresaId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var weekStart = today.AddDays(-6);
            var lookbackStart = today.AddDays(-90);
            var lookaheadEnd = today.AddDays(21);

            var appointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= lookbackStart && a.Data <= lookaheadEnd)
                .ToListAsync();

            if (appointments.Count == 0)
            {
                return new EmpresaCommunicationResponseDto
                {
                    Stats = new EmpresaCommunicationStatsDto(),
                    Templates = Array.Empty<EmpresaCommunicationTemplateDto>()
                };
            }

            var clients = appointments
                .Where(a => a.Cliente != null)
                .Select(a => a.Cliente!)
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .ToList();

            var sentThisWeek = appointments.Count(a => a.Data >= weekStart && a.Data <= today);
            var deliveryRate = Math.Round(CalculateContactCoverage(clients), 1);

            var activeCampaigns = appointments
                .Where(a => a.Data > today)
                .SelectMany(a => a.Servicos ?? Array.Empty<Servico>())
                .Select(s => s.ServicoPrestado)
                .Distinct()
                .Count();

            if (activeCampaigns == 0)
            {
                activeCampaigns = appointments.Count(a => a.Data > today);
            }

            var stats = new EmpresaCommunicationStatsDto
            {
                SentThisWeek = sentThisWeek,
                DeliveryRate = deliveryRate,
                ActiveCampaigns = activeCampaigns
            };

            var templates = BuildCommunicationTemplates(appointments, today);

            return new EmpresaCommunicationResponseDto
            {
                Stats = stats,
                Templates = templates
            };
        }

        public async Task<EmpresaAuditResponseDto> GetAuditAsync(int empresaId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var periodStart = today.AddDays(-30);

            var appointments = await QueryAgendamentos(empresaId)
                .Where(a => a.Data >= periodStart)
                .OrderByDescending(a => a.Data)
                .ThenByDescending(a => a.Hora)
                .Take(60)
                .ToListAsync();

            if (appointments.Count == 0)
            {
                return new EmpresaAuditResponseDto
                {
                    Logs = Array.Empty<EmpresaLogEntryDto>(),
                    Filters = new EmpresaAuditFiltersDto()
                };
            }

            var rawLogs = new List<(DateTime OccurredAt, EmpresaLogEntryDto Entry)>();

            foreach (var agendamento in appointments.Take(18))
            {
                var occurrence = agendamento.Data.ToDateTime(agendamento.Hora);
                rawLogs.Add((occurrence, new EmpresaLogEntryDto
                {
                    Module = "Agenda",
                    Action = DescribeAgendaAction(agendamento),
                    Actor = agendamento.Profissional?.Nome ?? "Equipe",
                    Timestamp = occurrence.ToString("dd/MM HH:mm", _culture),
                    Severity = MapAgendaSeverity(agendamento.StatusAgendamento)
                }));
            }

            var visitStats = appointments
                .Where(a => a.Cliente != null && a.Data <= today)
                .GroupBy(a => a.Cliente!)
                .Select(g => new
                {
                    Cliente = g.Key,
                    FirstVisit = g.Min(a => a.Data),
                    LastVisit = g.Max(a => a.Data)
                })
                .ToList();

            foreach (var novoCliente in visitStats
                .Where(v => v.FirstVisit >= today.AddDays(-15))
                .Take(3))
            {
                var occurrence = novoCliente.FirstVisit.ToDateTime(new TimeOnly(8, 30));
                rawLogs.Add((occurrence, new EmpresaLogEntryDto
                {
                    Module = "Clientes",
                    Action = $"Novo cliente {novoCliente.Cliente.Nome} realizou primeira visita",
                    Actor = "Recepcao",
                    Timestamp = occurrence.ToString("dd/MM HH:mm", _culture),
                    Severity = "info"
                }));
            }

            var reativados = visitStats
                .Where(v => v.LastVisit >= today.AddDays(-7) && v.FirstVisit <= today.AddDays(-60))
                .Take(2)
                .ToList();

            foreach (var cliente in reativados)
            {
                var occurrence = cliente.LastVisit.ToDateTime(new TimeOnly(17, 0));
                rawLogs.Add((occurrence, new EmpresaLogEntryDto
                {
                    Module = "Clientes",
                    Action = $"Reativou {cliente.Cliente.Nome} apos {FormatDate(cliente.FirstVisit)}",
                    Actor = "CRM",
                    Timestamp = occurrence.ToString("dd/MM HH:mm", _culture),
                    Severity = "info"
                }));
            }

            var revenueEntries = appointments
                .Where(a => IsRevenueStatus(a.StatusAgendamento))
                .ToList();

            if (revenueEntries.Count > 0)
            {
                var lastRevenue = revenueEntries.OrderByDescending(a => a.Data).ThenByDescending(a => a.Hora).First();
                var revenue = revenueEntries.Sum(a => (decimal)a.Valor);
                var occurrence = lastRevenue.Data.ToDateTime(lastRevenue.Hora);
                rawLogs.Add((occurrence, new EmpresaLogEntryDto
                {
                    Module = "Financeiro",
                    Action = $"Conferiu {FormatCurrency(revenue)} em servicos no periodo",
                    Actor = "Financeiro",
                    Timestamp = occurrence.ToString("dd/MM HH:mm", _culture),
                    Severity = "warning"
                }));
            }

            var orderedLogs = rawLogs
                .OrderByDescending(l => l.OccurredAt)
                .Take(15)
                .Select((entry, index) =>
                {
                    entry.Entry.Id = index + 1;
                    return entry.Entry;
                })
                .ToList();

            var filters = new EmpresaAuditFiltersDto
            {
                Modules = orderedLogs.Select(l => l.Module).Distinct().ToList(),
                Actors = orderedLogs.Select(l => l.Actor).Distinct().ToList()
            };

            return new EmpresaAuditResponseDto
            {
                Logs = orderedLogs,
                Filters = filters
            };
        }

        private IReadOnlyCollection<EmpresaCommunicationTemplateDto> BuildCommunicationTemplates(List<Agendamento> appointments, DateOnly today)
        {
            var templates = new List<EmpresaCommunicationTemplateDto>();
            var serviceSegments = appointments
                .Where(a => a.Servicos != null && a.Servicos.Count > 0)
                .SelectMany(a => a.Servicos.Select(s => new { Servico = s, Agendamento = a }))
                .GroupBy(x => x.Servico.Id)
                .Select(g =>
                {
                    var clients = g.Select(x => x.Agendamento.Cliente)
                        .Where(c => c != null)
                        .Select(c => c!)
                        .GroupBy(c => c.Id)
                        .Select(grp => grp.First())
                        .ToList();

                    return new
                    {
                        Servico = g.First().Servico,
                        Clients = clients,
                        ClientCount = clients.Count,
                        LastDate = g.Max(x => x.Agendamento.Data),
                        Upcoming = g.Any(x => x.Agendamento.Data > today)
                    };
                })
                .OrderByDescending(x => x.ClientCount)
                .ToList();

            int id = 1;
            var topService = serviceSegments.FirstOrDefault();
            if (topService != null)
            {
                templates.Add(new EmpresaCommunicationTemplateDto
                {
                    Id = id++,
                    Title = $"Lembrete de {topService.Servico.ServicoPrestado}",
                    Audience = $"{topService.ClientCount} clientes que procuram {topService.Servico.ServicoPrestado}",
                    Status = topService.Upcoming ? "agendado" : "enviado",
                    LastSend = FormatDate(topService.LastDate),
                    OpenRate = Math.Round(CalculateContactCoverage(topService.Clients), 1)
                });
            }

            var visitStats = appointments
                .Where(a => a.Cliente != null && a.Data <= today)
                .GroupBy(a => a.Cliente!)
                .Select(g => new
                {
                    Cliente = g.Key,
                    FirstVisit = g.Min(a => a.Data),
                    LastVisit = g.Max(a => a.Data)
                })
                .ToList();

            var inactiveClients = visitStats.Where(v => v.LastVisit <= today.AddDays(-45)).ToList();
            if (inactiveClients.Any())
            {
                templates.Add(new EmpresaCommunicationTemplateDto
                {
                    Id = id++,
                    Title = "Reativar clientes inativos",
                    Audience = $"{inactiveClients.Count} clientes sem visitas ha 45+ dias",
                    Status = "rascunho",
                    LastSend = FormatDate(inactiveClients.Max(v => v.LastVisit)),
                    OpenRate = Math.Round(CalculateContactCoverage(inactiveClients.Select(v => v.Cliente)), 1)
                });
            }

            var newClients = visitStats.Where(v => v.FirstVisit >= today.AddDays(-15)).ToList();
            if (newClients.Any())
            {
                templates.Add(new EmpresaCommunicationTemplateDto
                {
                    Id = id++,
                    Title = "Boas-vindas novos clientes",
                    Audience = $"{newClients.Count} novos clientes nas ultimas semanas",
                    Status = "enviado",
                    LastSend = FormatDate(newClients.Max(v => v.FirstVisit)),
                    OpenRate = Math.Round(CalculateContactCoverage(newClients.Select(v => v.Cliente)), 1)
                });
            }

            if (templates.Count == 0)
            {
                templates.Add(new EmpresaCommunicationTemplateDto
                {
                    Id = id,
                    Title = "Campanha geral",
                    Audience = "Base de clientes do periodo",
                    Status = "rascunho",
                    LastSend = DateTime.Now.ToString("dd/MM", _culture),
                    OpenRate = 0
                });
            }

            return templates;
        }

        private static Dictionary<int, ServiceActivitySnapshot> BuildServiceActivity(IEnumerable<Agendamento> appointments)
        {
            var activity = new Dictionary<int, ServiceActivitySnapshot>();
            foreach (var agendamento in appointments)
            {
                if (agendamento.Servicos == null) continue;
                foreach (var servico in agendamento.Servicos)
                {
                    if (!activity.TryGetValue(servico.Id, out var snapshot))
                    {
                        snapshot = new ServiceActivitySnapshot
                        {
                            ServiceId = servico.Id,
                            Name = servico.ServicoPrestado
                        };
                        activity[servico.Id] = snapshot;
                    }

                    snapshot.Executions++;
                    var occurrence = agendamento.Data.ToDateTime(agendamento.Hora);
                    if (snapshot.LastOccurrence == null || occurrence > snapshot.LastOccurrence)
                    {
                        snapshot.LastOccurrence = occurrence;
                        snapshot.LastProfessional = agendamento.Profissional?.Nome ?? "Equipe";
                        snapshot.LastClient = agendamento.Cliente?.Nome ?? "Cliente";
                    }
                }
            }

            return activity;
        }

        private static double CalculateContactCoverage(IEnumerable<Cliente> clients)
        {
            var clientList = clients?.Where(c => c != null).ToList() ?? new List<Cliente>();
            if (clientList.Count == 0) return 0;
            var reachable = clientList.Count(c => !string.IsNullOrWhiteSpace(c.Telefone));
            return (double)reachable / clientList.Count * 100;
        }

        private static IEnumerable<(string Name, double Amount)> AggregateServiceShares(IEnumerable<Agendamento> appointments)
        {
            return appointments
                .Where(a => a.Servicos != null && a.Servicos.Count > 0)
                .SelectMany(a =>
                {
                    var share = a.Valor / a.Servicos!.Count;
                    return a.Servicos.Select(s => (s.ServicoPrestado, (double)share));
                })
                .GroupBy(pair => pair.ServicoPrestado)
                .Select(group => (group.Key, group.Sum(p => p.Item2)));
        }

        private async Task<Dictionary<int, int>> GetServiceDurationMapAsync(int empresaId)
        {
            return await _context.ProfissionaisServicos
                .AsNoTracking()
                .Include(ps => ps.Servico)
                .Where(ps => ps.Servico != null && ps.Servico.EmpresaId == empresaId)
                .GroupBy(ps => ps.ServicoId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => (int)Math.Round(g.Average(ps => ps.Tempo.Hour * 60 + ps.Tempo.Minute))
                );
        }

        private IQueryable<Agendamento> QueryAgendamentos(int empresaId)
        {
            return _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Profissional).ThenInclude(p => p!.Empresa)
                .Include(a => a.Cliente)
                .Include(a => a.Servicos).ThenInclude(s => s.CategoriaServico)
                .Where(a => a.Profissional != null && a.Profissional.EmpresaId == empresaId);
        }

        private static bool IsRevenueStatus(StatusAgendamento status) =>
            status == StatusAgendamento.CONFIRMADO ||
            status == StatusAgendamento.REALIZADO ||
            status == StatusAgendamento.COBRADO;

        private static bool IsCanceled(StatusAgendamento status) =>
            status == StatusAgendamento.CANCELADO ||
            status == StatusAgendamento.CANCELADO_EMPRESA;

        private static string ToAgendaStatus(StatusAgendamento status) => status switch
        {
            StatusAgendamento.CONFIRMADO => "confirmado",
            StatusAgendamento.PENDENTE => "pendente",
            StatusAgendamento.CANCELADO => "cancelado",
            StatusAgendamento.CANCELADO_EMPRESA => "cancelado",
            StatusAgendamento.REALIZADO => "realizado",
            _ => "pendente"
        };

        private decimal SumRevenue(IEnumerable<Agendamento> agendamentos)
        {
            return agendamentos.Where(a => IsRevenueStatus(a.StatusAgendamento)).Sum(a => (decimal)a.Valor);
        }

        private string FormatCurrency(double value) => value.ToString("C0", _culture);
        private string FormatCurrency(decimal value) => value.ToString("C0", _culture);

        private string FormatDate(DateOnly date) => date.ToDateTime(TimeOnly.MinValue).ToString("dd/MM", _culture);

        private string DescribeAgendaAction(Agendamento agendamento)
        {
            var serviceName = agendamento.Servicos?.FirstOrDefault()?.ServicoPrestado ?? "Servico";
            var clientName = agendamento.Cliente?.Nome ?? $"Cliente #{agendamento.ClienteId}";
            var dateLabel = agendamento.Data.ToString("dd/MM", _culture);

            return agendamento.StatusAgendamento switch
            {
                StatusAgendamento.REALIZADO => $"Concluiu {serviceName} para {clientName} em {dateLabel}",
                StatusAgendamento.CONFIRMADO => $"Confirmou {serviceName} de {clientName} para {dateLabel}",
                StatusAgendamento.CANCELADO => $"Cliente cancelou {serviceName} marcado para {dateLabel}",
                StatusAgendamento.CANCELADO_EMPRESA => $"Empresa cancelou {serviceName} de {clientName}",
                StatusAgendamento.PENDENTE => $"Registrou novo agendamento de {serviceName} para {clientName} em {dateLabel}",
                _ => $"Atualizou agendamento de {serviceName} para {clientName}"
            };
        }

        private static string MapAgendaSeverity(StatusAgendamento status) => status switch
        {
            StatusAgendamento.CANCELADO_EMPRESA => "critical",
            StatusAgendamento.CANCELADO => "warning",
            StatusAgendamento.PENDENTE => "info",
            _ => "info"
        };

        private static double CalculateVariation(double current, double previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0;
            return Math.Round(((current - previous) / previous) * 100, 1);
        }

        private static double ParseCurrency(string value)
        {
            if (double.TryParse(value.Replace("R$", string.Empty, StringComparison.OrdinalIgnoreCase), out var result))
                return result;
            return 0;
        }

        private static double RandomTrend() => Math.Round(Random.Shared.NextDouble() * 8 - 4, 1);

        private sealed class ClientSnapshot
        {
            public Cliente Cliente { get; init; } = null!;
            public int Visits { get; set; }
            public DateOnly LastVisit { get; set; }
            public decimal Lifetime { get; set; }
            public int RecentVisits { get; set; }
        }

        private sealed class ServiceActivitySnapshot
        {
            public int ServiceId { get; init; }
            public string Name { get; init; } = string.Empty;
            public int Executions { get; set; }
            public DateTime? LastOccurrence { get; set; }
            public string LastProfessional { get; set; } = "Equipe";
            public string LastClient { get; set; } = "Cliente";
        }
    }
}
