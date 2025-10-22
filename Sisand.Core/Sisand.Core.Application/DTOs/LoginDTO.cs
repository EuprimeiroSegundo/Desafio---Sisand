using System.ComponentModel.DataAnnotations;
using Sisand.Core.Domain.Entities;

namespace Sisand.Core.Application.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "O campo de login/email é obrigatório.")]
    public required string LoginEmail { get; set; }

    [Required(ErrorMessage = "O campo de senha é obrigatório.")]
    public required string Senha { get; set; }
}
