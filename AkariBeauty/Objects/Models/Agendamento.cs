using AkariBeauty.Objects.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkariBeauty.Objects.Models
{
    [Table("agendamento")]
    public class Agendamento
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("data")]
        public DateOnly Data { get; set; }

        [Column("hora")]
        public TimeOnly Hora { get; set; }

        [Column("valor")]
        public float Valor { get; set; }

        [Column("comissao")]
        public float Comissao { get; set; }

        [Column("status_agendamento")]
        public StatusAgendamento StatusAgendamento { get; set; }

        [Column("clienteid")][ForeignKey("cliente")]
        public int ClienteId { get; set; }

        [Column("empresaid")][ForeignKey("empresa")]
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public Empresa? Empresa { get; set; }
        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        public Agendamento() { }

        public Agendamento(int id, DateOnly data, TimeOnly hora, float valor, float comissao, StatusAgendamento statusAgendamento, int clienteId, int empresaId)
        {
            Id = id;
            Data = data;
            Hora = hora;
            Valor = valor;
            Comissao = comissao;
            StatusAgendamento = statusAgendamento;
            ClienteId = clienteId;
            EmpresaId = empresaId;
        }
    } 
}