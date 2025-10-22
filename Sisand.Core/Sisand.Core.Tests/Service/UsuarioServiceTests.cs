using Moq;
using Sisand.Core.Application.DTOs;
using Sisand.Core.Application.Exceptions;
using Sisand.Core.Domain.Entities;
using Sisand.Core.Domain.Interfaces;
using Sisand.Core.Application.DTOs;
using Sisand.Core.Application.Services;

namespace Sisand.Core.Tests.Service;

public class UsuarioServiceTests
{

    private readonly Mock<IUsuarioRepostirory> _repository;
    private readonly UsuarioService _service;
    private readonly UsuarioModel usuarioModel;
    private readonly UsuarioDTO usuarioDTO;
    private readonly CadastrarUsuarioDTO cadastroUsuarioDTO;

    public UsuarioServiceTests()
    {
        _repository = new();
        _service = new(_repository.Object);

        usuarioModel = new()
        {
            UsuarioId = 1,
            Login = "LoginTest",
            Senha = "SenhaTest",
            Email = "EmailTest",
            NomeCompleto = "NomeCompletoTest",
            Telefone = "41922222222",
            Permissao = "visitante"
        };

        cadastroUsuarioDTO = new()
        {
            Senha = "SenhaTest",
            Login = "LoginTest",
            Email = "EmailTest",
            NomeCompleto = "NomeCompletoTest",
            Telefone = "41922222222",
            Permissao = "visitante"
        };

        usuarioDTO = UsuarioDTO.From(usuarioModel);
    }

    [Fact(DisplayName = "Valiidação com sucesso de busca de usuário por login ou email.")]
    public async Task BuscarUsuarioPorLoginEmail_ComSucesso()
    {
        // Arrange
        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioModel);

        // Act
        UsuarioModel result = await _service.BuscarUsuarioPorLoginEmailAsync("Teste");

        //
        Assert.Equal(result, usuarioModel);
    }

    [Fact(DisplayName = "Valiidação com exceção de busca de usuário por login ou email, normalização do login falha.")]
    public async Task BuscarUsuarioPorLoginEmail_LancamentoExcecao_Nomalizacao()
    {
        // Arrage & Act
        Exception exception = await Assert.ThrowsAsync<Exception>(async () => await _service.BuscarUsuarioPorLoginEmailAsync(""));

        // Assert
        Assert.Equal("Nenhum dos campos pode ser vazio ou nulo.", exception.Message);
    }

    [Fact(DisplayName = "Validação com sucesso de listagem de usuários.")]
    public async Task ListarUsuarios_ComSucesso()
    {
        // Arrange
        _repository.Setup(r => r.ObterTodosAsync()).ReturnsAsync([usuarioModel]);

        // Act
        List<UsuarioDTO> result = await _service.ListarUsuarios();

        // Assert
        Assert.Equal(usuarioDTO.Email, result[0].Email);
        Assert.Equal(usuarioDTO.Login, result[0].Login);
        Assert.Equal(usuarioDTO.NomeCompleto, result[0].NomeCompleto);
        Assert.Equal(usuarioDTO.Permissao, result[0].Permissao);
        Assert.Equal(usuarioDTO.Telefone, result[0].Telefone);
    }

    [Fact(DisplayName = "Validação com exceção de listagem de usuários, sem nenhum usuário encontrado.")]
    public async Task ListarUsuarios_LancamentoExcecao_NoContent()
    {
        // Arrange
        _repository.Setup(r => r.ObterTodosAsync()).ReturnsAsync((List<UsuarioModel>)[]);

        // Act & Assert
        NoContentException exception = await Assert.ThrowsAsync<NoContentException>(async () => await _service.ListarUsuarios());
    }

    [Fact(DisplayName = "Validação com exceção de listagem de usuários, lançamento exceção genérica.")]
    public async Task ListarUsuarios_LancamentoExcecao_ExceptionGenerica()
    {
        // Arrange
        _repository.Setup(r => r.ObterTodosAsync()).Throws(new Exception());

        // Act & Assert
        Exception exception = await Assert.ThrowsAsync<Exception>(async () => await _service.ListarUsuarios());
    }

    [Fact(DisplayName = "Validação com sucesso de cadastrar usuário.")]
    public async Task CadastrarUsuario_ComSucesso()
    {
        // Arrange
        string permissao = "admin";

        // Act
        CadastrarUsuarioDTO result = await _service.CadastrarUsuarioAsync(cadastroUsuarioDTO, permissao);

        // Assert
        Assert.Equal(result, cadastroUsuarioDTO);
    }

    [Fact(DisplayName = "Validação com exceção de cadastrar usuário, erro de permissionamento.")]
    public async Task CadastrarUsuario_LancamentoExcecao_BaixoPermissionamento()
    {
        // Arrange
        string permissao = "visitante";

        // Act 
        Exception exception = await Assert.ThrowsAsync<Exception>(async () => await _service.CadastrarUsuarioAsync(cadastroUsuarioDTO, permissao));

        // Assert
        Assert.Equal("Nível da permissão logada não permite a ação.", exception.Message);
    }

    [Fact(DisplayName = "Validação com exceção de cadastrar usuário, ausência de permissão de permissão válida.")]
    public async Task CadastrarUsuario_LancamentoExcecao_AusenciaDePermissaoValida()
    {
        // Arrange
        string permissao = "PermissaoInválida";

        // Act 
        Exception exception = await Assert.ThrowsAsync<Exception>(async () => await _service.CadastrarUsuarioAsync(cadastroUsuarioDTO, permissao));

        // Assert
        Assert.Equal("Uma permissão inválida foi passada.", exception.Message);
    }

    [Fact(DisplayName = "Validação com sucesso de atualizar usuário, como administador.")]
    public async Task AtualizarUsuario_ComSucesso_ComoAdministrador()
    {
        // Arrange
        string permissao = "admin";
        string login = "LoginTest";

        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioModel);

        // Act
        UsuarioDTO result = await _service.AtualizarUsuarioAsync(cadastroUsuarioDTO, permissao, login);

        // Assert
        Assert.Equal(result, cadastroUsuarioDTO);
    }

    [Fact(DisplayName = "Validação com sucesso de atualizar usuário, como usuário.")]
    public async Task AtualizarUsuario_ComSucesso_ComoUsuario()
    {
        // Arrange
        string permissao = "usuario";
        string login = "LoginTest";
        UsuarioModel usuarioAtualizar = new()
        {
            UsuarioId = 1,
            Login = "LoginTest",
            Senha = "SenhaTest",
            Email = "EmailTest",
            NomeCompleto = "NomeCompletoTest",
            Telefone = "41922222222",
            Permissao = "visitante"
        };

        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioAtualizar);

        // Act
        UsuarioDTO result = await _service.AtualizarUsuarioAsync(cadastroUsuarioDTO, permissao, login);

        // Assert
        Assert.Equal(result, cadastroUsuarioDTO);
    }

    [Fact(DisplayName = "Validação com exceção de atualizar usuário, erro de permissionamento.")]
    public async Task AtualizarUsuario_LancamentoExcecao_BaixoPermissionamento()
    {
        // Arrange
        string permissao = "visitante";
        string login = "LoginTest";
        UsuarioModel usuarioAtualizar = new()
        {
            UsuarioId = 1,
            Login = "LoginTest",
            Senha = "SenhaTest",
            Email = "EmailTest",
            NomeCompleto = "NomeCompletoTest",
            Telefone = "41922222222",
            Permissao = "visitante"
        };
        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioAtualizar);

        // Act 
        Exception exception = await Assert.ThrowsAsync<Exception>(async () => await _service.AtualizarUsuarioAsync(cadastroUsuarioDTO, permissao, login));

        // Assert
        Assert.Equal("Nível da permissão logada não permite a ação.", exception.Message);
    }

    [Fact(DisplayName = "Validação com sucesso de deletar usuário, como administrador.")]
    public async Task DeletarUsuario_ComSucesso_ComoAdministrador()
    {
        // Arrange
        string permissao = "admin";
        string loginDeletar = "LoginTest";
        string login = "LoginTest";
        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioModel);

        // Act
        await _service.DeletarUsuarioAsync(loginDeletar, permissao, login);

        // Assert
        Assert.True(true);
    }

    [Fact(DisplayName = "Validação com sucesso de deletar usuário, como usuário.")]
    public async Task DeletarUsuario_ComSucesso_ComoUsuario()
    {
        // Arrange
        string permissao = "usuario";
        string loginDeletar = "LoginTest";
        string login = "LoginTest";


        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioModel);

        // Act
        await _service.DeletarUsuarioAsync(loginDeletar, permissao, login);

        // Assert
        Assert.True(true);
    }

    [Fact(DisplayName = "Validação com exceção de deletar usuário, erro de permissionamento.")]
    public async Task DeletarUsuario_LancamentoExcecao_BaixoPermissionamento()
    {
        // Arrange
        string permissao = "visitante";
        string loginDeletar = "LoginTest";
        string login = "LoginTest";
        _repository.Setup(r => r.BuscarUsuarioPorLoginEmailAsync(It.IsAny<string>())).ReturnsAsync(usuarioModel);

        // Act
        Exception exception = await Assert.ThrowsAsync<Exception>(async () => await _service.DeletarUsuarioAsync(loginDeletar, permissao, login));

        // Assert
        Assert.Equal("Nível da permissão logada não permite a ação.", exception.Message);
    }
}
