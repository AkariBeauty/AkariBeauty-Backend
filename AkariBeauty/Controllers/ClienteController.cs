using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AkariBeauty.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _service;
    public ClienteController(IClienteService service) => _service = service;

    // SIGNUP (público)
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] Cliente body)
    {
        await _service.Create(body); // service.Create é Task (void)
        return StatusCode(201);
    }

    // LOGIN (público)
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] RequestLoginDTO req)
    {
        var token = await _service.Login(req);
        return Ok(new { token });
    }

    // ALTERAR SENHA (protegido)
    [HttpPost("profile/change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO request)
    {
        var identifier = User.FindFirstValue("clienteId") ?? User.FindFirstValue("identifier") ??
                         User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(identifier, out var clienteId))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        try
        {
            await _service.ChangePasswordAsync(clienteId, request.CurrentPassword, request.NewPassword);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao alterar senha.");
        }
    }

    // LISTAR (protegido)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAll();
        return Ok(list);
    }

    // GET ID (protegido)
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _service.GetById(id);
        return Ok(dto);
    }

    // UPDATE (protegido)
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] Cliente body)
    {
        await _service.Update(body, id);
        return NoContent();
    }

    // DELETE (protegido)
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Remove(id);
        return NoContent();
    }

    // PERFIL (protegido) - usa claim clienteId do JWT
    [HttpGet("perfil")]
    [Authorize]
    public async Task<IActionResult> GetPerfil()
    {
        var clienteIdStr = User.FindFirst("clienteId")?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(clienteIdStr) || !int.TryParse(clienteIdStr, out var id))
            return Unauthorized();

        var dto = await _service.GetById(id);
        return Ok(dto);
    }

    [HttpPatch("perfil")]
    [Authorize]
    public async Task<IActionResult> UpdatePerfil([FromBody] Cliente body)
    {
        var clienteIdStr = User.FindFirst("clienteId")?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(clienteIdStr) || !int.TryParse(clienteIdStr, out var id))
            return Unauthorized();

        body.Id = id;
        await _service.Update(body, id);
        return NoContent();
    }
}
