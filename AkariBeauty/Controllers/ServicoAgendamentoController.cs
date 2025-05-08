using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoAgendamentoController : Controller
    {
    }
}
