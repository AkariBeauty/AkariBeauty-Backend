using AkariBeauty.Objects.Dtos.Agendamentos;
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var agendamento = await _agendamentoService.GetById(id);
            if (agendamento == null)
                return NotFound("Agendamento não encontrado");
            return Ok(agendamento);
        }

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var itens = await _agendamentoService.GetByClienteId(clienteId);
            return Ok(itens);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CriarAgendamentoRequest request)
        {
            try
            {
                var criado = await _agendamentoService.CreateAgendamentoAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir um novo agendamento");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] CriarAgendamentoRequest _)
        {
            return StatusCode(501, "Atualização de agendamento ainda não foi implementada.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _agendamentoService.Remove(id);
                return Ok("Agendamento cancelado com sucesso");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar cancelar o agendamento");
            }
        }
    }
}
