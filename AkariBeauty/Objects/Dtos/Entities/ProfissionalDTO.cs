using AkariBeauty.Objects.DataAnnotations.Formats;
using AkariBeauty.Objects.DataAnnotations.Validations;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Dtos.Entities;

public class ProfissionalDTO
{
    public int Id { get; set; }

    public string Nome { get; set; }

    [ValidateNullOrEmpty(ErrorMessage = "O CPF nao pode ser nulo ou vazio")]
    [ValidateQtdCaracters(11, 14, ErrorMessage = "O CPF deve ter 11 ou 14 caracteres")]
    [FormatCpf]
    public string Cpf { get; set; }

    [ValidateNullOrEmpty(ErrorMessage = "O salario nao pode ser nulo ou vazio")]
    public float Salario { get; set; }

    [FormatRemoveSpaceStartEnd]
    public string Login { get; set; }

    [ValidatePassword]
    public string Senha { get; set; }

    [ValidateNullOrEmpty(ErrorMessage = "O telefone nao pode ser nulo ou vazio")]
    [ValidateQtdCaracters(16, 11, ErrorMessage = "O telefone deve ter 11 ou 16 caracteres")]
    [FormatPhone]
    public string Telefone { get; set; }

    public int EmpresaId { get; set; }

    public StatusProfissional Status { get; set; }

}