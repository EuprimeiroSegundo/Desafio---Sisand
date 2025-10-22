using Microsoft.EntityFrameworkCore;
using Sisand.Core.Data.Context;
using Sisand.Core.Domain.Entities;
using Sisand.Core.Domain.Interfaces;

namespace Sisand.Core.Data.Repositories;

public class UsuarioRepository(SisandDbContext _context) : BaseRepository<UsuarioModel>(_context), IUsuarioRepostirory
{
    public async Task<UsuarioModel?> BuscarUsuarioPorLoginEmailAsync(string loginEmail)
    {
        return await _context.Usuario.FirstOrDefaultAsync(u => u.Login == loginEmail || u.Email == loginEmail);
    }
}
