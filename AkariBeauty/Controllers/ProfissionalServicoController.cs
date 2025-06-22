using System.Threading.Tasks;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionalServicoController : Controller
    {

        IProfissionalServicoService _service;

        public ProfissionalServicoController(IProfissionalServicoService service)
        {
            _service = service;
        }

        [HttpGet("profissional-servico/{idProfissional:int}")]
        public async Task<IActionResult> ProfissionalServico(int idProfissional)
        {
            try
            {
                var profissionalServico = await _service.GetProfissionalServicoForProfissional(idProfissional);
                return Ok(profissionalServico);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao acessar o servidor: " + ex.Message);
            }


        }
        
        [HttpGet("servico-profissional/{idServico:int}")]
        public async Task<IActionResult> SevicoProfissional(int idServico)
        {
            try
            {
                var servicoProfissional = await _service.GetProfissionalServicoForServico(idServico);
                return Ok(servicoProfissional);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao acessar o servidor: " + ex.Message);
            }
            
        }

    }
}
