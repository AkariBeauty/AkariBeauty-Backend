namespace AkariBeauty.Objects.Dtos.Entities
{
    public class ServicoDTO
    {
        public string Id { get; set; }
        public string ServicoPrestado { get; set; }
        public string Descricao { get; set; }
        public TimeOnly TempoBase { get; set; }
        public string ValorBase { get; set; }
        public int EmpresaId { get; set; }

    }
}
