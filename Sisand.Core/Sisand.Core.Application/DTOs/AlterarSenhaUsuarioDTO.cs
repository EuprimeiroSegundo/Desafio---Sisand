using System.ComponentModel.DataAnnotations;

namespace Sisand.Core.Application.DTOs;

public class AlterarSenhaUsuarioDTO
{
    public required string Login { get; set; }
    
    [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
    [StringLength(150, MinimumLength = 8, ErrorMessage = "O campo 'Senha' deve possuir pelo menos oito caracteres.")]
    public required string NovaSenha { get; set; }
}
