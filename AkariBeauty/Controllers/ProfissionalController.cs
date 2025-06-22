using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Dtos.DataAnnotations.Base;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Controllers
{
    // [Authorize] // Uncomment this line if you want to secure all endpoints by default
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProfissionalController : Controller
    {
        private readonly IProfissionalService _service;
        private readonly IMapper _mapper;

        public ProfissionalController(IProfissionalService profissionalService, IMapper mapper)
        {
            _service = profissionalService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var profissionais = await _service.GetAll();
                return Ok(profissionais);
            }
            catch (ArgumentException ex)
            {
                // Returning NotFound is generally more appropriate if no data is found due to an argument,
                // otherwise a BadRequest might be better for invalid input.
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Generic 500 error for unhandled exceptions
                return StatusCode(500, "Ocorreu um erro ao acessar o servidor: " + ex.Message);
            }
        }

        [HttpGet("{id:int}")] // Corrected: Removed space around :
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var profissional = await _service.GetById(id);

                return Ok(profissional);
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

        [HttpPost]
        public async Task<IActionResult> Post(ProfissionalDTO profissional)
        {
            try
            {
                ExecuteAnnotation.Executar(profissional);

                await _service.Create(_mapper.Map<Profissional>(profissional));

                return Ok(profissional);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar inserir um novo profissional: " + ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id,  ProfissionalDTO profissionalDto)
        {
            try
            {
                if (id != profissionalDto.Id)
                    return BadRequest("O ID na rota não corresponde ao ID do profissional no corpo da requisição.");

                ExecuteAnnotation.Executar(profissionalDto);

                await _service.Update(_mapper.Map<Profissional>(profissionalDto), id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar os dados do profissional: " + ex.Message);
            }
            return Ok(profissionalDto);
        }

        [HttpDelete("{id:int}")] // Corrected: Added :int constraint
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Remove(id);
            }
            catch (KeyNotFoundException ex) // Catch specific KeyNotFoundException from service
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex) // Catch ArgumentException for other invalid arguments
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar remover um profissional: " + ex.Message);
            }
            return Ok("Profissional removido com sucesso");
        }

        [AllowAnonymous]
        [HttpPatch("login")]
        public async Task<IActionResult> Login(RequestLoginDTO request)
        {
            try
            {
                var token = await _service.Login(request);
                return Ok(token);
            }
            catch (ArgumentException ex) // For invalid credentials, return Unauthorized
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex) // Catch other generic exceptions
            {
                return StatusCode(500, "Erro ao acessar o servidor: " + ex.Message);
            }
        }

        [HttpPost("add-servico")]
        public async Task<IActionResult> AddServico(ProfissionalServico request)
        {
            try
            {
                await _service.AddServico(request);
                return Ok("Serviço adicionado ao profissional com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao adicionar serviço ao profissional: " + ex.Message);
            }
        }

        [HttpGet("{idProfissional:int}/servicos")] // Corrected: Removed spaces around :
        public async Task<IActionResult> GetAllServicos(int idProfissional)
        {
            try
            {
                var servicos = await _service.GetAllSevicos(idProfissional);
                return Ok(servicos);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar serviços do profissional: " + ex.Message);
            }
        }

        [HttpDelete("{idProfissional:int}/remove-servico/{idServico:int}")] // Corrected: Removed spaces around :
        public async Task<IActionResult> RemoveServico(int idProfissional, int idServico)
        {
            try
            {
                await _service.RemoveServico(idProfissional, idServico);
                return Ok("Serviço removido do profissional com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao remover serviço do profissional: " + ex.Message);
            }
        }
    }
}
