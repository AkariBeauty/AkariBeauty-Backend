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

        // Cliente
        [Column("cliente_id")]
        public int ClienteId { get; set; }
        [ForeignKey("Id")]
        public Cliente Cliente { get; set; }

        // Serviço N : N

        // // Profissional
        // [Column("profissional_id")]
        // public int ProfissionalId { get; set; }
        // [ForeignKey("Id")]
        // public Profissional Profissional { get; set; }

        // ContaReceber

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