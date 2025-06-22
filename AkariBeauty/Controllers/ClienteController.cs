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
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService cliente)
        {
            this._clienteService = cliente;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAll();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.GetById(id);
            if (cliente == null)
                return NotFound("Cliente não encontrado");
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Cliente cliente)
        {
            try
            {
                await _clienteService.Create(cliente);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir um novo cliente");
            }
            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cliente cliente)
        {
            try
            {
                await _clienteService.Update(cliente, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar os dados do cliente: " + ex.Message);
            }
            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.Remove(id);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar remover o cliente");
            }
            return Ok("Cliente removido com sucesso");
        }

        [HttpPatch("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(RequestLoginDTO request)
        {
            try
            {
                var token = await _clienteService.Login(request);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao acessar o servidor: " + ex.Message);
            };
        }
    }
}
