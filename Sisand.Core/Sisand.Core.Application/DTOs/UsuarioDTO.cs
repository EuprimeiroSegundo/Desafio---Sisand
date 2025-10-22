using System.ComponentModel.DataAnnotations;
using Sisand.Core.Domain.Entities;

namespace Sisand.Core.Application.DTOs;

public class UsuarioDTO
{
    [Required(ErrorMessage = "O campo 'Login' é obrigatório.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo 'login' deve possuir pelo menos três caracteres.")]
    public required string Login { get; set; }

    [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
    [StringLength(50)]
    public required string Email { get; set; }

    [Required(ErrorMessage = "O campo 'NomeCompleto' é obrigatório.")]
    [StringLength(150, MinimumLength = 6, ErrorMessage = "O campo 'NomeCompleto' deve possuir pelo menos seis caracteres.")]
    public required string NomeCompleto { get; set; }

    [Required(ErrorMessage = "O campo 'Telefone' é obrigatório.")]
    [StringLength(50, MinimumLength = 11, ErrorMessage = "O campo 'Telefone' deve possuir pelo menos onze caracteres.")]
    public required string Telefone { get; set; }

    [Required(ErrorMessage = "O campo 'Permissao' é obrigatório.")]
    [StringLength(25, MinimumLength = 4, ErrorMessage = "O campo 'Permissão' deve possuir pelo menos quatro caracteres.")]
    public required string Permissao { get; set; }

    public static UsuarioDTO From(UsuarioModel usuario)
    {
        return new UsuarioDTO
        {
            Login = usuario.Login,
            Email = usuario.Email,
            NomeCompleto = usuario.NomeCompleto,
            Telefone = usuario.Telefone,
            Permissao = usuario.Permissao
        };
    }
}
