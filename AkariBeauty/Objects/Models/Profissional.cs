using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Relationship;

namespace AkariBeauty.Objects.Models;

[Table("profissional")]
public class Profissional
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("nome")]
    public string Nome { get; set; }
    [Column("cpf")]
    public string Cpf { get; set; }
    [Column("salario")]
    public float Salario { get; set; }
    [Column("login")]
    public string Login { get; set; }
    [Column("senha")]
    public string Senha { get; set; }
    [Column("telefone")]
    public string Telefone { get; set; }
    [Column("status")]
    public StatusProfissional Status { get; set; }
    
    [JsonIgnore]
    public Empresa? Empresa { get; set; }
    [Column("empresa_id")]
    [ForeignKey("Empresa")]
    public int EmpresaId { get; set; }
    [JsonIgnore]
    public ICollection<ProfissionalServico>? ProfissionalServicos { get; set; } = new List<ProfissionalServico>();
    [JsonIgnore]
    public ICollection<Agendamento>? Agendamentos { get; set; } = new List<Agendamento>();

    public Profissional() { }

    public Profissional(int id, string nome, string cpf, float salario, string login, string senha, string telefone, int empresaId, StatusProfissional status)
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
        Salario = salario;
        Login = login;
        Senha = senha;
        Telefone = telefone;
        EmpresaId = empresaId;
        Status = status;
    }

}