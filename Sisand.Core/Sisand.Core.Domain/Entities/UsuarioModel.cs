using System.ComponentModel.DataAnnotations;

namespace Sisand.Core.Domain.Entities;

public class UsuarioModel : BaseModel
{
    [Key]
    public int UsuarioId { get; set; }
    public required string Login { get; set; }
    public required string Senha { get; set; }
    public required string Email { get; set; }
    public required string NomeCompleto { get; set; }
    public required string Telefone { get; set; }
    public required string Permissao { get; set; }
}
