using System.ComponentModel.DataAnnotations;

namespace AkariBeauty.Objects.Dtos.Agendamentos;

public class CriarAgendamentoRequest
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int ServicoId { get; set; }

    [Required]
    public int ProfissionalId { get; set; }

    [Required]
    public DateTime DataHora { get; set; }

    public string? Observacao { get; set; }
}
