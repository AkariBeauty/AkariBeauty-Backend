namespace AkariBeauty.Objects.Dtos.Agendamentos;

public class AgendamentoDetalheDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = string.Empty;
    public float Valor { get; set; }
    public float Comissao { get; set; }
    public IEnumerable<ServicoResumoDTO> Servicos { get; set; } = Enumerable.Empty<ServicoResumoDTO>();
}
