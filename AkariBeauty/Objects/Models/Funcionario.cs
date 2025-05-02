using System.ComponentModel.DataAnnotations.Schema;

namespace AkariBeauty.Objects.Models
{
    [Table("funcionario")]
    public class Funcionario
    {
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

        public Funcionario() { }

        public Funcionario(int id, string nome, string cpf, float salario, string login, string senha)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Salario = salario;
            Login = login;
            Senha = senha;
        }
    }
}
