using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Controllers.Dtos;

public class AgendamentoPorAnoDTO
{
    public int Ano { get; set; }
    public int? AnoEnd { get; set; }
    public StatusAgendamento? statusAgendamento { get; set; }
}
