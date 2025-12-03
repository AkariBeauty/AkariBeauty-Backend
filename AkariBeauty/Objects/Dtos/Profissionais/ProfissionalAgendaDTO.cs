using System;
using System.Collections.Generic;
using System.Linq;
using AkariBeauty.Objects.Dtos.Agendamentos;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Dtos.Profissionais;

public class ProfissionalAgendaDiaDTO
{
    public DateOnly Data { get; set; }
    public IEnumerable<ProfissionalAgendaItemDTO> Agendamentos { get; set; } = Enumerable.Empty<ProfissionalAgendaItemDTO>();
}

public class ProfissionalAgendaItemDTO
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string? ClienteTelefone { get; set; }
    public string ServicoPrincipal { get; set; } = string.Empty;
    public StatusAgendamento StatusCodigo { get; set; }
    public string Status { get; set; } = string.Empty;
    public float Valor { get; set; }
    public string? Observacao { get; set; }
    public bool PodeConfirmar { get; set; }
    public bool PodeConcluir { get; set; }
}

public class ProfissionalAgendamentoDetalheDTO : ProfissionalAgendaItemDTO
{
    public int ClienteId { get; set; }
    public IEnumerable<ServicoResumoDTO> Servicos { get; set; } = Enumerable.Empty<ServicoResumoDTO>();
}
