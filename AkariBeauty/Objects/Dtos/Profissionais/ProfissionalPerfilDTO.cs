using System.ComponentModel.DataAnnotations;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Dtos.Profissionais;

public class ProfissionalPerfilDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public int EmpresaId { get; set; }
    public string? EmpresaNome { get; set; }
    public StatusProfissional StatusCodigo { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class AtualizarProfissionalPerfilDTO
{
    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Login { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    [StringLength(60, MinimumLength = 6)]
    public string? Senha { get; set; }
}
