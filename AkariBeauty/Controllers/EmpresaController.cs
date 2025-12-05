using System.Security.Claims;
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
        private readonly IEmpresaInsightsService _empresaInsightsService;

        public EmpresaController(IEmpresaService empresaService, IEmpresaInsightsService empresaInsightsService)
        {
            this._empresaService = empresaService;
            _empresaInsightsService = empresaInsightsService;
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetDashboardAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("profissionais")]
        public async Task<IActionResult> Professionals([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetProfessionalsAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("servicos")]
        public async Task<IActionResult> Services([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetServicesAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("agenda")]
        public async Task<IActionResult> Agenda([FromQuery] DateOnly? inicio = null, [FromQuery] DateOnly? fim = null, [FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetAgendaAsync(targetId, inicio, fim);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Clients([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetClientsAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("financeiro")]
        public async Task<IActionResult> Finance([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetFinanceAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("config")]
        [HttpGet("configuracoes")]
        public async Task<IActionResult> Settings([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetSettingsAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("comunicacao")]
        public async Task<IActionResult> Communication([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetCommunicationAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("auditoria")]
        public async Task<IActionResult> Audit([FromQuery] int? empresaId = null)
        {
            try
            {
                var targetId = ResolveEmpresaId(empresaId);
                var payload = await _empresaInsightsService.GetAuditAsync(targetId);
                return Ok(payload);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        private int ResolveEmpresaId(int? empresaId)
        {
            if (empresaId.HasValue)
            {
                return empresaId.Value;
            }

            var claimValue = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "empresaId", StringComparison.OrdinalIgnoreCase))?.Value;
            if (int.TryParse(claimValue, out var claimId))
            {
                return claimId;
            }

            throw new InvalidOperationException("Empresa não encontrada no token do usuário");
        }
    }
}
