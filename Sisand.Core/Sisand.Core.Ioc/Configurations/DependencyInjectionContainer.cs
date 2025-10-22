using System;
using Microsoft.Extensions.DependencyInjection;
using Sisand.Core.Application.Interfaces.Autenticacao;
using Sisand.Core.Application.Services.Autenticacao;
using Sisand.Core.Data.Repositories;
using Sisand.Core.Domain.Interfaces;
using Sisand.Core.Application.Interfaces;
using Sisand.Core.Application.Services;

namespace Sisand.Core.Ioc.Configurations;

public class DependencyInjectionContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUsuarioRepostirory, UsuarioRepository>();

        // Services
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAutenticacaoService, AutenticacaoService>();
    }
}
