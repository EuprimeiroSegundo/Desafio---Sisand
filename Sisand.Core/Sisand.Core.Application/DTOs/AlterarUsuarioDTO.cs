using System.ComponentModel.DataAnnotations;
using Sisand.Core.Application.DTOs;
using Sisand.Core.Domain.Entities;

namespace Sisand.Core.Application.DTOs;

public class CadastrarUsuarioDTO : UsuarioDTO
{    
    [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
    [StringLength(150, MinimumLength = 8, ErrorMessage = "O campo 'Senha' deve possuir pelo menos oito caracteres.")]
    public required string Senha { get; set; }

    public static CadastrarUsuarioDTO From(UsuarioModel usuario)
    {
        return new CadastrarUsuarioDTO
        {
            Login = usuario.Login,
            Senha = usuario.Senha,
            Email = usuario.Email,
            NomeCompleto = usuario.NomeCompleto,
            Telefone = usuario.Telefone,
            Permissao = usuario.Permissao
        };
    }
}
