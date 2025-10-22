using Microsoft.EntityFrameworkCore;
using Sisand.Core.Data.Context;
using Sisand.Core.Domain.Interfaces;

namespace Sisand.Core.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly SisandDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(SisandDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = _context.Set<T>();
    }

    public async Task<List<T>> ObterTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> ObterPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task InserirAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(object key)
    {
        var entity = await _context.Set<T>().FindAsync(key);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
