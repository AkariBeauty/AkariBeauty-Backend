using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgendamentoController : Controller
    {
        private readonly IAgendamentoService _agendamentoService;

        public AgendamentoController(IAgendamentoService agendamento)
        {
            this._agendamentoService = agendamento;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var agendamentos = await _agendamentoService.GetAll();
            return Ok(agendamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var agendamentos = await _agendamentoService.GetById(id);
            if (agendamentos == null)
                return NotFound("Agendamento não encontrado");
            return Ok(agendamentos);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Agendamento agendamento)
        {
            try
            {
                await _agendamentoService.Create(agendamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir um novo agendamento");
            }
            return Ok(agendamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Agendamento agendamento)
        {
            try
            {
                await _agendamentoService.Update(agendamento, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar os dados do agendamento: " + ex.Message);
            }
            return Ok(agendamento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _agendamentoService.Remove(id);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar remover um agendamento");
            }
            return Ok("Agendamento removido com sucesso");
        }
    }
}
