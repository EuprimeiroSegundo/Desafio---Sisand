using System;
using Sisand.Core.Application.DTOs;

namespace Sisand.Core.Application.DTOs.Autenticacao;

public class LoginResponse
{
    public required UsuarioDTO usuario { get; set; }
    public required string token { get; set; }
}
