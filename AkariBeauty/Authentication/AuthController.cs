using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            var token = _authService.Authenticate(loginDto.Username, loginDto.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Credenciais inválidas!");
            }

            return Ok(new { Token = token });
        }
    }
}
