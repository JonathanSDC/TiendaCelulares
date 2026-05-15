using Microsoft.AspNetCore.Mvc;
using Tienda_Celulares.ApiService.Services;

namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /*
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Username, request.Password);
            if (user == null) return Unauthorized("Usuario o contraseña incorrectos");

            return Ok(new
            {
                user.IdUsuario,
                user.Username,
                Roles = user.UsuarioRoles.Select(r => r.Rol.NombreRol).ToList()
            });
        } */

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authService.LoginAsync(request.Username, request.Password);
                if (user == null) return Unauthorized("Usuario o contraseña incorrectos");

                return Ok(new
                {
                    user.IdUsuario,
                    user.Username,
                    Roles = user.UsuarioRoles.Select(r => r.Rol.NombreRol).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detail = ex.ToString() });
            }
        }


       [HttpPost("test")]
        public IActionResult Test()
        {
            var hash = _authService.LoginAsync("admin", "1234").Result;
            return Ok(hash);
        }
       



    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
