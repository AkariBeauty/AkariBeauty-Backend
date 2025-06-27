using System;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Controllers.Dtos;

public class AgendamentoPorMesDTO
{
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int? MesEnd { get; set; }
    public int? AnoEnd { get; set; }
    public StatusAgendamento? statusAgendamento { get; set; }
}
