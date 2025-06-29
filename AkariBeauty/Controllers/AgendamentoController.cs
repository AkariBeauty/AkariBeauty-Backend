using AkariBeauty.Controllers.Dtos;
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

        [HttpGet("filtrar")] // Endpoint único e claro
        public async Task<IActionResult> GetAgendamentosFiltrados([FromQuery] RequestAgendamentoForDateDTO filtro)
        {
            try
            {
                string token = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token não fornecido.");
                }

                if (!filtro.Ano.HasValue)
                {
                    return BadRequest("Ano é obrigatório para qualquer filtro de data.");
                }

                var requestAgendamento = new RequestAgendamentoForDateDTO();

                if (filtro.Semana.HasValue)
                {
                    requestAgendamento.Semana = filtro.Semana;
                    requestAgendamento.Ano = filtro.Ano;
                    requestAgendamento.Mes = filtro.Mes;
                }
                else
                {
                    requestAgendamento.Ano = filtro.Ano;
                    requestAgendamento.Mes = filtro.Mes ?? 1;
                    requestAgendamento.Dia = filtro.Dia ?? 1; // Padrão: dia 1 se não for informado

                    requestAgendamento.AnoEnd = filtro.AnoEnd ?? filtro.Ano;
                    requestAgendamento.MesEnd = filtro.MesEnd ?? filtro.Mes ?? 12;
                    requestAgendamento.DiaEnd = filtro.DiaEnd ?? filtro.Dia;
                }

                requestAgendamento.StatusAgendamento = filtro.StatusAgendamento;

                var agendamentos = await _agendamentoService.GetAgendamentosPorData(requestAgendamento);
                return Ok(agendamentos);
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
