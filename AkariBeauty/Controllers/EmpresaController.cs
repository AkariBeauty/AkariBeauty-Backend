using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmpresaController : Controller
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            this._empresaService = empresaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var empresas = await _empresaService.GetAll();
            return Ok(empresas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empresa = await _empresaService.GetById(id);
            if (empresa == null)
                return NotFound("Empresa não encontrada");
            return Ok(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmpresaComUsuarioDTO dto)
        {
            try
            {
                await _empresaService.Create(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir uma nova empresa");
            }
            return Ok(dto.Empresa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Empresa empresa)
        {
            try
            {
                await _empresaService.Update(empresa, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar os dados da empresa: " + ex.Message);
            }
            return Ok(empresa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _empresaService.Remove(id);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar remover a empresa");
            }
            return Ok("Empresa removida com sucesso");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RequestLogin request)
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            throw new NotImplementedException();
        }
    }
}
