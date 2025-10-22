using Microsoft.Extensions.Options;
using Moq;
using Sisand.Core.Application.Interfaces.Autenticacao;
using Sisand.Core.Utils.Autenticacao;
using Sisand.Core.Application.Interfaces;

namespace Sisand.Core.Tests.Service;

public class AutenticacaoServiceTests
{
    private readonly Mock<IUsuarioService> _usuarioService;
    private readonly Mock<IOptions<JwtSettings>> _ioptionsJwtSettings;
    private readonly IAutenticacaoService _autenticacaoService;

    public AutenticacaoServiceTests()
    {
        _usuarioService = new();
        // _autenticacaoService = new();
    }

}
