using System;

namespace AkariBeauty.Objects.Dtos.Agendamentos
{
    public class DisponibilidadeFiltro
    {
        public int ServicoId { get; set; }
        public int? ProfissionalId { get; set; }
        public DateOnly? Inicio { get; set; }
        public DateOnly? Fim { get; set; }
    }
}
