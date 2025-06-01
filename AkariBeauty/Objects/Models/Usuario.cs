using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        public string Nome { get; set; }

        [Required]
        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("salario")]
        public float Salario { get; set; }

        [Required]
        [Column("login")]
        public string Login { get; set; }

        [Required]
        [Column("senha")]
        public string Senha { get; set; }

        [Required]
        [Column("tipo_usuario")]
        public TipoUsuario TipoUsuario { get; set; }

        [Required]
        [Column("empresa_id")]
        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }

        [JsonIgnore]
        public Empresa? Empresa { get; set; }

        // [JsonIgnore]
        // public ICollection<ContaPagar>? ContasPagar { get; set;}
        public Usuario() { }

        public Usuario(int id, string nome, string cpf, float salario, string login, string senha, TipoUsuario tipoUsuario, int empresaId)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Salario = salario;
            Login = login;
            Senha = senha;
            TipoUsuario = tipoUsuario;
            EmpresaId = empresaId;
        }
    }
}
