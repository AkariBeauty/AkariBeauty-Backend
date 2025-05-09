using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkariBeauty.Objects.Models
{
    [Table("funcionario")]
    public class Funcionario
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

        [Column("empresa_id")][ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public Empresa? Empresa { get; set; }

        // [JsonIgnore]
        // public ICollection<ContaPagar>? ContasPagar { get; set;}
        public Funcionario() { }

        public Funcionario(int id, string nome, string cpf, float salario, string login, string senha, int empresaId)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Salario = salario;
            Login = login;
            Senha = senha;
            EmpresaId = empresaId;
        }
    }
}
