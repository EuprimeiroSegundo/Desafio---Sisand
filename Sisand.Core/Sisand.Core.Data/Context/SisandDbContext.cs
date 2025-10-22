using Microsoft.EntityFrameworkCore;
using Sisand.Core.Domain.Entities;

namespace Sisand.Core.Data.Context;

public class SisandDbContext : DbContext
{
    public SisandDbContext(DbContextOptions options) : base(options){}

    public virtual DbSet<UsuarioModel> Usuario { get; set; }
}
