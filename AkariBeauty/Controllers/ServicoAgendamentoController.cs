using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoAgendamentoController : Controller
    {
        private readonly IServicoAgendamentoService _aSService;

        public ServicoAgendamentoController(IServicoAgendamentoService aSService)
        {
            this._aSService = aSService;
        }

        [HttpPost("vincular")]
        public async Task<IActionResult> Vincular(int agendamentoId, int servicoId)
        {
            try
            {
                await _aSService.VincularServicoAoAgendamento(agendamentoId, servicoId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar desvincular um serviço do agendamento: " + ex.Message);
            }
            return Ok("Servico vinculado ao agendamento com sucesso");
        }

        [HttpDelete("desvincular")]
        public async Task<IActionResult> Desvincular(int agendamentoId, int servicoId)
        {
            try
            {
                await _aSService.DesvincularServicoDoAgendamento(agendamentoId, servicoId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar desvincular um serviço ao agendamento: " + ex.Message);
            }
            return Ok("Servico desvinculado ao agendamento com êxito");
        }
    }
}
