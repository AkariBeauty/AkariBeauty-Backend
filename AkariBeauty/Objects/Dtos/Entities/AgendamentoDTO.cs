using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Dtos.Entities
{
    public class AgendamentoDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
        public float Valor { get; set; }
        public float Comissao { get; set; }
        public StatusAgendamento StatusAgendamento { get; set; }
        public int ClienteId { get; set; }
        public int EmpresaId { get; set; }
    }
}
