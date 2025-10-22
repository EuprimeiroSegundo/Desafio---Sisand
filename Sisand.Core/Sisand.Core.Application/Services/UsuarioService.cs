using Sisand.Core.Application.DTOs;
using Sisand.Core.Application.Enums;
using Sisand.Core.Application.Exceptions;
using Sisand.Core.Domain.Entities;
using Sisand.Core.Domain.Interfaces;
using Sisand.Core.Utils.Permissao;
using Sisand.Core.Application.Interfaces;

namespace Sisand.Core.Application.Services;

public class UsuarioService(IUsuarioRepostirory _repository) : IUsuarioService
{
    public async Task<UsuarioModel?> BuscarUsuarioPorLoginEmailAsync(string loginEmail)
    {
        try
        {
            string loginEmaiilNormalizado = NomalizacaoCampo(loginEmail);
            var usuario = await _repository.BuscarUsuarioPorLoginEmailAsync(loginEmaiilNormalizado);

            return usuario;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<UsuarioDTO>> ListarUsuarios()
    {
        try
        {
            List<UsuarioDTO> usuariosRetorno = [];
            var usuarios = await _repository.ObterTodosAsync();

            if (!usuarios.Any())
                throw new NoContentException();

            foreach (var usuario in usuarios)
            {
                UsuarioDTO usuarioDTO = UsuarioDTO.From(usuario);
                usuariosRetorno.Add(usuarioDTO);
            }

            return usuariosRetorno;
        }
        catch (NoContentException)
        {
            throw new NoContentException();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<CadastrarUsuarioDTO> CadastrarUsuarioAsync(CadastrarUsuarioDTO cadastroUsuarioDTO, string permissao)
    {
        try
        {
            var usuarioExistente = await BuscarUsuarioPorLoginEmailAsync(cadastroUsuarioDTO.Login);

            if (usuarioExistente != null)
                throw new Exception("Login já existente, não pode ser cadastrado novamente.");

            string SenhaHash = PasswordToHash(cadastroUsuarioDTO.Senha);
            var usuario = new UsuarioModel
            {
                Login = NomalizacaoCampo(cadastroUsuarioDTO.Login),
                Senha = SenhaHash,
                Email = NomalizacaoCampo(cadastroUsuarioDTO.Email),
                NomeCompleto = NomalizacaoCampo(cadastroUsuarioDTO.NomeCompleto),
                Telefone = NomalizacaoCampo(cadastroUsuarioDTO.Telefone),
                Permissao = NomalizacaoCampo(cadastroUsuarioDTO.Permissao),
                CriadoEm = DateTime.Now,
                AlteradoEm = DateTime.Now
            };

            ValidarNivelPermissao(cadastroUsuarioDTO.Permissao, permissao, AcoesEnum.Criar);

            await _repository.InserirAsync(usuario);

            return cadastroUsuarioDTO;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UsuarioDTO> AtualizarUsuarioAsync(UsuarioDTO usuarioDTO, string permissao, string login)
    {
        try
        {
            var usuario = await BuscarUsuarioPorLoginEmailAsync(usuarioDTO.Login) ?? throw new Exception("Usuário não encontrado.");

            usuario.Login = usuarioDTO.Login;
            usuario.Email = usuarioDTO.Email;
            usuario.NomeCompleto = usuarioDTO.NomeCompleto;
            usuario.Telefone = usuarioDTO.Telefone;
            usuario.Permissao = usuarioDTO.Permissao;

            ValidarNivelPermissao(usuarioDTO.Permissao, permissao, AcoesEnum.Editar, [usuarioDTO.Login, login]);

            await _repository.AtualizarAsync(usuario);

            return usuarioDTO;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AlterarSenhaUsuarioAsync(AlterarSenhaUsuarioDTO usuarioDTO, string permissao, string login)
    {
        var usuario = await BuscarUsuarioPorLoginEmailAsync(usuarioDTO.Login) ?? throw new Exception("Usuário não encontrado.");

        ValidarNivelPermissao(usuario.Permissao, permissao, AcoesEnum.Editar, [usuarioDTO.Login, login]);

        string SenhaHash = PasswordToHash(usuarioDTO.NovaSenha);
        usuario.Senha = SenhaHash;

        await _repository.AtualizarAsync(usuario);

        return;
    }

    public async Task DeletarUsuarioAsync(string loginDeletar, string permissao, string login)
    {
        try
        {
            var usuario = await BuscarUsuarioPorLoginEmailAsync(loginDeletar) ?? throw new Exception("Usuário não encontrado.");

            ValidarNivelPermissao(usuario.Permissao, permissao, AcoesEnum.Deletar, [loginDeletar, login]);

            await _repository.DeletarAsync(usuario.UsuarioId);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static void ValidarNivelPermissao(string cadastrarPermissao, string loginPermissao, AcoesEnum acao, List<string>? logins = null )
    {
        string erroPermissionamento = "Nível da permissão logada não permite a ação.";

        if (
            !Permissoes.permissoes.TryGetValue(cadastrarPermissao, out int valorInserir) ||
            !Permissoes.permissoes.TryGetValue(loginPermissao, out int valorLogin)
        )
            throw new Exception("Uma permissão inválida foi passada.");

        switch (acao)
        {
            case AcoesEnum.Criar:
                {
                    if (
                        valorLogin < valorInserir ||
                        valorLogin == Permissoes.permissoes["visitante"]
                        )
                        throw new Exception(erroPermissionamento);
                    break;
                }
            case AcoesEnum.Editar or AcoesEnum.Deletar or AcoesEnum.Senha:
                {
                    if (
                        valorLogin < valorInserir ||
                        valorLogin == Permissoes.permissoes["visitante"] ||
                        valorLogin == Permissoes.permissoes["usuario"] && logins[0] != logins[1]
                        )
                        throw new Exception(erroPermissionamento);
                    break;
                }
        }
    }

    private static string PasswordToHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private string NomalizacaoCampo(string valor)
    {
        string campoNormalizado = valor.ToLower().Trim();

        if (string.IsNullOrWhiteSpace(campoNormalizado))
            throw new Exception("Nenhum dos campos pode ser vazio ou nulo.");

        return campoNormalizado;
    }
}
