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
