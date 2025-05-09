using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FuncionarioController : Controller
    {

        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionario)
        {
            this._funcionarioService = funcionario;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var funcionarios = await _funcionarioService.GetAll();
            return Ok(funcionarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var funcionarios = _funcionarioService.GetById(id);
            if (funcionarios == null)
                return NotFound("Funcionário não encontrado");
            return Ok(funcionarios);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Funcionario funcionario)
        {
            try
            {
                await _funcionarioService.Create(funcionario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir um novo funcionário");
            }
            return Ok(funcionario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Funcionario funcionario)
        {
            try
            {
                await _funcionarioService.Update(funcionario, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar os dados do funcionário" + ex.Message);
            }
            return Ok(funcionario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _funcionarioService.Remove(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar remover um funcionário");
            }
            return Ok("Funcionário removido com sucesso");
        }
    }
}
