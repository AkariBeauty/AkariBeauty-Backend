using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;



namespace AkariBeauty.Objects.Models
{
    [Table("servico")]
    public class Servico
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("servicoprestado")]
        public string ServicoPrestado { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("valorbase")]
        public float ValorBase { get; set; }

        [Column("empresa_id")][ForeignKey("Empresa")]
        public int EmpresaId { get; set; }

        [JsonIgnore]
        public Empresa? Empresa { get; set; }

        public Servico() { }

        public Servico(int id, string servicoPrestado, string descricao, float valorBase, int empresaId)
        {
            Id = id;
            ServicoPrestado = servicoPrestado;
            Descricao = descricao;
            ValorBase = valorBase;
            EmpresaId = empresaId;
        }
    }
}
