using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AkariBeauty.Objects.Models;

[Table("categoria_servico")]
public class CategoriaServico
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("categoria")]
    public string? Categoria { get; set; }
    [Column("descricao")]
    public string? Descricao { get; set; }
    [JsonIgnore]
    public ICollection<Servico>? Servicos { get; set; } = new List<Servico>();

    public CategoriaServico() { }

    public CategoriaServico(int id, string categoria, string descricao)
    {
        Id = id;
        Categoria = categoria;
        Descricao = descricao;
    }

}
