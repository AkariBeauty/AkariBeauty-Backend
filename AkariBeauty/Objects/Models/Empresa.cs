using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkariBeauty.Objects.Models
{
    [Table("empresa")]
    public class Empresa
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cnpj")]
        public string Cnpj { get; set; }

        [Column("razao_social")]
        public string RazaoSocial { get; set; }

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

        [Column("hora_inicial")]
        public TimeOnly HoraInicial { get; set; }

        [Column("hora_final")]
        public TimeOnly HoraFinal { get; set; }

        [Column("adiantamento")]
        public bool Adiantamento { get; set; }

        [JsonIgnore]
        public ICollection<Servico>? Servicos { get; set; } = new List<Servico>();

        [JsonIgnore]
        public ICollection<Funcionario>? Funcionarios { get; set; } = new List<Funcionario>();

        // [JsonIgnore]
        // public ICollection<Despesa>? Despesas { get; set;} = new List<Despesa>();

        // [JsonIgnore]
        // public ICollection<Profissional>? Profissionais { get; set; } = new List<Profissional>();

        public Empresa() { }

        public Empresa(int id, string cnpj, string razaoSocial, string uf, string cidade, string bairro, string rua, int numero, TimeOnly horaInicial, TimeOnly horaFinal, bool adiantamento)
        {
            Id = id;
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Uf = uf;
            Cidade = cidade;
            Bairro = bairro;
            Rua = rua;
            Numero = numero;
            HoraInicial = horaInicial;
            HoraFinal = horaFinal;
            Adiantamento = adiantamento;
        }
    }
}
