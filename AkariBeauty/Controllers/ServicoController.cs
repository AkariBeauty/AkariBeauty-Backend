using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServicoController : Controller
    {
        private readonly IServicoService _servicoService;

        public ServicoController(IServicoService servico)
        {
            this._servicoService = servico;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var servicos = await _servicoService.GetAll();
            return Ok(servicos);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var servicos = await _servicoService.GetById(id);
            if (servicos == null)
                return NotFound("Serviço não encontrado");
            return Ok(servicos);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Servico servico)
        {
            try
            {
                await _servicoService.Create(servico);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir um novo serviço");
            }
            return Ok(servico);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Servico servico)
        {
            try
            {
                await _servicoService.Update(servico, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar os dados do serviço" + ex.Message);
            }
            return Ok(servico);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _servicoService.Remove(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar remover um serviço");
            }
            return Ok("Serviço removido com sucesso");
        }
    }
}
