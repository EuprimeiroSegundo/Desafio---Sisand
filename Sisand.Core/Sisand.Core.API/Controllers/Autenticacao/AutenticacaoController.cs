using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sisand.Core.Application.DTOs.Autenticacao;
using Sisand.Core.Application.Interfaces.Autenticacao;
using Sisand.Core.Utils.Autenticacao;
using Sisand.Core.Application.DTOs;

namespace Sisand.Core.API.Controllers.Autenticacao
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController(IAutenticacaoService _service) : ControllerBase
    {

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUsuario(IOptions<JwtSettings> _jwtSettings, LoginDTO login)
        {
            try
            {
                LoginResponse loginResponse = await _service.Login(login);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(_jwtSettings.Value.Expiration)
                };

                Response.Cookies.Append("token", loginResponse.token, cookieOptions);

                return Ok(new {loginResponse.usuario});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
