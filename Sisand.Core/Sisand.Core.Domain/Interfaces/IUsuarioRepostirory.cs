using Sisand.Core.Domain.Entities;

namespace Sisand.Core.Domain.Interfaces;

public interface IUsuarioRepostirory : IBaseRepository<UsuarioModel>
{
    public Task<UsuarioModel?> BuscarUsuarioPorLoginEmailAsync(string loginEmail);
}
