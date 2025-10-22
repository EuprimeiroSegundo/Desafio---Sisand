using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sisand.Core.Application.DTOs;
using Sisand.Core.Application.DTOs.Autenticacao;
using Sisand.Core.Application.Exceptions;
using Sisand.Core.Application.Interfaces.Autenticacao;
using Sisand.Core.Domain.Entities;
using Sisand.Core.Utils.Autenticacao;
using Sisand.Core.Application.DTOs;
using Sisand.Core.Application.Interfaces;

namespace Sisand.Core.Application.Services.Autenticacao;

public class AutenticacaoService(IOptions<JwtSettings> _jwtSettings, IUsuarioService _usuarioService) : IAutenticacaoService
{
    public async Task<LoginResponse> Login(LoginDTO loginDTO)
    {
        try
        {
            UsuarioModel usuario = await _usuarioService.BuscarUsuarioPorLoginEmailAsync(loginDTO.LoginEmail) ?? throw new Exception("Usuário não encontrado.");

            if (!VerificarSenhaHash(loginDTO.Senha, usuario.Senha))
                throw new NotFoundException("login/email ou senha incorretos.");

            LoginResponse loginResponse = GerarJwtToken(usuario);

            return loginResponse;
        }
        catch (NotFoundException ex)
        {
            throw new Exception("login/email ou senha incorretos.");
        }
        catch (Exception)
        {
            throw new Exception("Houve um problema ao realizar o login.");
        }
    }

    public bool ValidarJwtToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Value.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Value.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private LoginResponse GerarJwtToken(UsuarioModel usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, usuario.Permissao)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.Value.Expiration),
            signingCredentials: creds
        );

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        LoginResponse response = new()
        {
            usuario = UsuarioDTO.From(usuario),
            token = tokenString
        };

        return response;
    }

    private static bool VerificarSenhaHash(string passwordLogin, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(passwordLogin, passwordHash);
        }
        catch
        {
            return false;
        }
    }
}
