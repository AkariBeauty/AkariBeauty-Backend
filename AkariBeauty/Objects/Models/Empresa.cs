using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkariBeauty.Objects.Models
{
    [Table("empresa")]
    public class Empresa
    {
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
