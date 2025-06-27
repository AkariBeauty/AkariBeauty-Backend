    using AkariBeauty.Objects.Enums;
    using AkariBeauty.Services.Types;
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

            [JsonConverter(typeof(DateOnlyJsonConverter))]
            [Column("data")]
            public DateOnly Data { get; set; }

            [Column("dia_semana")]
            public int DiaSemana { get; set; }

            [Column("hora")]
            public TimeOnly Hora { get; set; }

            [Column("valor")]
            public float Valor { get; set; }

            [Column("comissao")]
            public float Comissao { get; set; }

            [Column("status_agendamento")]
            public StatusAgendamento StatusAgendamento { get; set; }

            // Cliente
            [Column("cliente_id")][ForeignKey("Cliente")]
            public int ClienteId { get; set; }
            [JsonIgnore]
            public Cliente? Cliente { get; set; }

            // Serviço
            [JsonIgnore]
            public ICollection<Servico> Servicos { get; set; } = new List<Servico>();

            [Column("profissional_id")][ForeignKey("Profissional")]
            public int ProfissionalId { get; set; }
            [JsonIgnore]
            public Profissional? Profissional { get; set; }

            // TODO: ContaReceber
            // [JsonIgnore]
            // public ContaReceber ContaReceber { get; set; }

            public Agendamento() { }

            public Agendamento(int id, DateOnly data, int diaSemana, TimeOnly hora, float valor, float comissao, StatusAgendamento statusAgendamento, int clienteId, int profissionalId)
            {
                Id = id;
                Data = data;
                Hora = hora;
                DiaSemana = diaSemana;
                Valor = valor;
                Comissao = comissao;
                StatusAgendamento = statusAgendamento;
                ClienteId = clienteId;
                ProfissionalId = profissionalId;
            }
        } 
    }