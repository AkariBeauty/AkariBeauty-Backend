using System;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Controllers.Dtos;

public class AgendamentoPorSemanaDTO
{
    public int Semana { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public StatusAgendamento? statusAgendamento { get; set; }
}
