using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkariBeauty.Objects.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("uf")]
        public string Uf { get; set; }

        [Column("cidade")]
        public string Cidade { get; set; }

        [Column("bairro")]
        public string Bairro { get; set; }

        [Column("rua")]
        public string Rua { get; set; }

        [Column("numero")]
        public int Numero { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        [Column("telefone")]
        public string Telefone { get; set; }

        [JsonIgnore]
        public ICollection<Agendamento>? Agendamentos { get; set; }

        public Cliente() { }

        public Cliente(int id, string nome, string cpf, string uf, string cidade, string bairro, string rua, int numero, string login, string senha, string telefone)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Uf = uf;
            Cidade = cidade;
            Bairro = bairro;
            Rua = rua;
            Numero = numero;
            Login = login;
            Senha = senha;
            Telefone = telefone;
        }
    }
}
