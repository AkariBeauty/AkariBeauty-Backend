using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    // [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriaServicoController : Controller
    {
        private readonly ICategoriaServicoService _service;

        public CategoriaServicoController(ICategoriaServicoService service)
        {
            _service = service;
        }

        // GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categorias = await _service.GetAll();
                return Ok(categorias);
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

        // GetById
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var categoria = await _service.GetById(id);
                return Ok(categoria);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao acessar o servidor: " + ex.Message);
            }
        }
    }
}
