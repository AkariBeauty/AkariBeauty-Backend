using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Relationship;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



namespace AkariBeauty.Objects.Models
{
    [Table("servico")]
    public class Servico
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("servicoprestado")]
        public string ServicoPrestado { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("tempobase")]
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly TempoBase { get; set; }

        [Column("valorbase")]
        public float ValorBase { get; set; }
        [Column("empresa_id")]
        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public Empresa? Empresa { get; set; }

        [JsonIgnore]
        public ICollection<Agendamento>? Agendamentos { get; set; } = new List<Agendamento>();

        [JsonIgnore]
        public ICollection<ProfissionalServico>? ProfissionalServicos { get; set; } = new List<ProfissionalServico>();

        [Column("categoria_servico_id")][ForeignKey("CategoriaServico")]
        public int CategoriaServicoId { get; set;}
        [JsonIgnore]
        public CategoriaServico? CategoriaServico { get; set; }

        public Servico() { }

        public Servico(int id, string servicoPrestado, string descricao, float valorBase, TimeOnly timeBase, int empresaId, int categoriaServicoId)
        {
            Id = id;
            ServicoPrestado = servicoPrestado;
            Descricao = descricao;
            ValorBase = valorBase;
            TempoBase = timeBase;
            EmpresaId = empresaId;
            CategoriaServicoId = categoriaServicoId;
        }
    }
}
