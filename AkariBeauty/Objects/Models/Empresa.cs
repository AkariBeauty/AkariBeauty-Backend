using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AkariBeauty.Services.Types;

namespace AkariBeauty.Objects.Models
{
    [Table("empresa")]
    public class Empresa
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [JsonIgnore]
        [NotMapped]
        private string _cnpj { get; set; }

        [Column("cnpj")]
        public string Cnpj
        {
            get => _cnpj;
            set => _cnpj = CnpjValidade.Validar(value);
        }

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

        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        [Column("hora_inicial")]
        public TimeOnly HoraInicial { get; set; }

        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        [Column("hora_final")]
        public TimeOnly HoraFinal { get; set; }

        [Column("adiantamento")]
        public bool Adiantamento { get; set; }

        [JsonIgnore]
        public ICollection<Servico>? Servicos { get; set; } = new List<Servico>();

        [JsonIgnore]
        public ICollection<Usuario>? Usuarios { get; set; } = new List<Usuario>();

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
