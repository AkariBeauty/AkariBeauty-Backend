using System.ComponentModel.DataAnnotations;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Dtos.Profissionais;

public class AtualizarStatusAgendamentoDTO
{
    [Required]
    public StatusAgendamento NovoStatus { get; set; }

    [MaxLength(500)]
    public string? Justificativa { get; set; }
}
