using System.Collections.Generic;
using System.Linq;

namespace AkariBeauty.Objects.Dtos.Profissionais;

public class ProfissionalDashboardDTO
{
    public string Nome { get; set; } = string.Empty;
    public int PendentesHoje { get; set; }
    public int ConfirmadosHoje { get; set; }
    public int TotalSemana { get; set; }
    public int CanceladosSemana { get; set; }
    public IEnumerable<ProfissionalAgendaItemDTO> Proximos { get; set; } = Enumerable.Empty<ProfissionalAgendaItemDTO>();
}
