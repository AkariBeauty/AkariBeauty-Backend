using AkariBeauty.Objects.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkariBeauty.Objects.Models
{
    [Table("agendamento")]
    public class Agendamento
    {
        [Key]
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

        // Cliente
        [Column("cliente_id")][ForeignKey("Id")]
        public int ClienteId { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; }

        // Serviço
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();

        // TODO: Profissional
        // [Column("profissional_id")][ForeignKey("Id")]
        // public int ProfissionalId { get; set; }
        // [JsonIgnore]
        // public Profissional Profissional { get; set; }

        // TODO: ContaReceber
        // [JsonIgnore]
        // public ContaReceber ContaReceber { get; set; }

        public Agendamento() { }

        public Agendamento(int id, DateOnly data, TimeOnly hora, float valor, float comissao, StatusAgendamento statusAgendamento, int clienteId)
        {
            Id = id;
            Data = data;
            Hora = hora;
            Valor = valor;
            Comissao = comissao;
            StatusAgendamento = statusAgendamento;
            ClienteId = clienteId;
        }
    } 
}