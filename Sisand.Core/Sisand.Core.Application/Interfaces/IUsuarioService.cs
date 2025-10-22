using Sisand.Core.Application.DTOs;
using Sisand.Core.Domain.Entities;

namespace Sisand.Core.Application.Interfaces;

public interface IUsuarioService
{
    public Task<UsuarioModel> BuscarUsuarioPorLoginEmailAsync(string loginEmail);
    public Task<List<UsuarioDTO>> ListarUsuarios();
    public Task<CadastrarUsuarioDTO> CadastrarUsuarioAsync(CadastrarUsuarioDTO cadastroUsuarioDTO, string permissao);
    public Task<UsuarioDTO> AtualizarUsuarioAsync(UsuarioDTO usuarioDTO, string permissao, string login);
    public Task AlterarSenhaUsuarioAsync(AlterarSenhaUsuarioDTO usuarioDTO, string permissao, string login);
    public Task DeletarUsuarioAsync(string loginDeletar, string permissao, string login);
}
