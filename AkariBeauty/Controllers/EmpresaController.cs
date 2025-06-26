using System.Runtime.CompilerServices;
using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Post(EmpresaComUsuarioDTO dto)
        {
            try
            {
                var empresa = dto.Empresa;
                empresa.Usuarios?.Add(dto.Usuario);
                var retorno = await _empresaService.Create(empresa);

                return Ok(retorno);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir uma nova empresa" + ex.Message);
            }
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

        [HttpPatch("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(RequestLoginDTO request)
        {
            try
            {
                var token = await _empresaService.Login(request);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao acessar o servidor: " + ex.Message);
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                string token = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token nao fornecido");

                var empresa = await _empresaService.GetUser(token);
                return Ok(empresa);
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
