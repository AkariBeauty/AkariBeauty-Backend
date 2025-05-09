using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Objects.Relationship;

public class ProfissionalServico
{
    [Column("profissional_id")][Key]
    public int ProfissionalId { get; set; }

    [Column("servico_id")][Key]
    public int ServicoId { get; set; }

    // [JsonIgnore]
    // public Profissional? Profissional { get; set; }

    [JsonIgnore]
    public Servico? Servico { get; set; }

    [Column("comissao")]
    public float Comissao { get; set; }

    [Column("tempo")]
    public TimeOnly Tempo { get; set; }

    public ProfissionalServico() { }

    public ProfissionalServico(int profissionalId, int servicoId, float comissao, TimeOnly tempo)
    {
        ProfissionalId = profissionalId;
        ServicoId = servicoId;
        Comissao = comissao;
        Tempo = tempo;
    }
}
