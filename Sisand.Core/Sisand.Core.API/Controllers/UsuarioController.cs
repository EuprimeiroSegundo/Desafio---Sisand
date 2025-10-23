using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sisand.Core.Application.DTOs;
using Sisand.Core.Application.Exceptions;
using Sisand.Core.Domain.Entities;
using Sisand.Core.Application.Interfaces;


namespace Sisand.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController(IUsuarioService _service) : ControllerBase
    {
        [HttpGet("BuscarUsuarioPorLoginEmail/{loginEmail}")]
        public async Task<IActionResult> BuscarUsuarioPorLoginEmail([FromRoute] string loginEmail)
        {
            try
            {
                UsuarioModel usuario = await _service.BuscarUsuarioPorLoginEmailAsync(loginEmail) ?? throw new Exception("Usuário não encontrado.");

                UsuarioDTO usuarioResponse = UsuarioDTO.From(usuario);

                return Ok(usuarioResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarUsuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                return Ok(await _service.ListarUsuarios());
            }
            catch (NoContentException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario(CadastrarUsuarioDTO usuarioDTO)

        {
            try
            {
                var usuario = HttpContext.User;
                var roleClaim = usuario.FindFirst(ClaimTypes.Role);
                string permissao;

                if (roleClaim != null)
                {
                    permissao = roleClaim.Value;
                }
                else
                {
                    throw new Exception("O nível de permissão do usuário logado e login não foram encontrados.");
                }

                return Ok(await _service.CadastrarUsuarioAsync(usuarioDTO, permissao));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AtualizarUsuario")]
        public async Task<IActionResult> AtualizarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = HttpContext.User;
                var roleClaim = usuario.FindFirst(ClaimTypes.Role);
                var loginClaim = usuario.FindFirst(ClaimTypes.NameIdentifier);
                string permissao, login;

                if (roleClaim != null && loginClaim != null)
                {
                    permissao = roleClaim.Value;
                    login = loginClaim.Value;
                }
                else
                {
                    throw new Exception("O nível de permissão do usuário logado e login não foram encontrados.");
                }

                return Ok(await _service.AtualizarUsuarioAsync(usuarioDTO, permissao, login));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AlterarSenhaUsuario")]
        public async Task<IActionResult> AlterarSenhaUsuario(AlterarSenhaUsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = HttpContext.User;
                var roleClaim = usuario.FindFirst(ClaimTypes.Role);
                var loginClaim = usuario.FindFirst(ClaimTypes.NameIdentifier);
                string permissao, login;

                if (roleClaim != null && loginClaim != null)
                {
                    permissao = roleClaim.Value;
                    login = loginClaim.Value;
                }
                else
                {
                    throw new Exception("O nível de permissão do usuário logado e login não foram encontrados.");
                }

                await _service.AlterarSenhaUsuarioAsync(usuarioDTO, permissao, login);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarUsuario/{loginDeletar}")]
        public async Task<IActionResult> DeletarUsuario(string loginDeletar)
        {
            try
            {
                var usuario = HttpContext.User;
                var roleClaim = usuario.FindFirst(ClaimTypes.Role);
                var loginClaim = usuario.FindFirst(ClaimTypes.NameIdentifier);
                string permissao, login;

                if (roleClaim != null && loginClaim != null)
                {
                    permissao = roleClaim.Value;
                    login = loginClaim.Value;
                }
                else
                {
                    throw new Exception("O nível de permissão do usuário logado e login não foram encontrados.");
                }

                await _service.DeletarUsuarioAsync(loginDeletar, permissao, login);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
