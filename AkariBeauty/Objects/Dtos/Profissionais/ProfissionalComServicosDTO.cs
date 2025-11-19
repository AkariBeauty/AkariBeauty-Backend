using System.Collections.Generic;
using System.Linq;
using AkariBeauty.Objects.Dtos.Agendamentos;

namespace AkariBeauty.Objects.Dtos.Profissionais;

public class ProfissionalComServicosDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public IEnumerable<ProfissionalComServicosRelacaoDTO> ProfissionalServicos { get; set; } = Enumerable.Empty<ProfissionalComServicosRelacaoDTO>();
}

public class ProfissionalComServicosRelacaoDTO
{
    public int ServicoId { get; set; }
    public ServicoNomeDTO? Servico { get; set; }
}

public class ServicoNomeDTO
{
    public string? ServicoPrestado { get; set; }
}
