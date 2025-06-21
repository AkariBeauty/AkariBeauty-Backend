namespace AkariBeauty.Objects.Dtos.Entities;

public class ProfissionalServicoDTO
{
    public int ProfissionalId { get; set; }
    public int ServicoId { get; set; }
    public float Comissao { get; set; }
    public TimeOnly Tempo { get; set; }
}
