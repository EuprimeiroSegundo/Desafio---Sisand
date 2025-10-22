using Sisand.Core.Application.DTOs.Autenticacao;
using Sisand.Core.Application.DTOs;

namespace Sisand.Core.Application.Interfaces.Autenticacao;

public interface IAutenticacaoService
{
    public Task<LoginResponse> Login(LoginDTO loginDTO);
    public bool ValidarJwtToken(string token);
}
