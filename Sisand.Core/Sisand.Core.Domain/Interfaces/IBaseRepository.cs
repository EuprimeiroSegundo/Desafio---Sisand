namespace Sisand.Core.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>> ObterTodosAsync();

    Task<T?> ObterPorIdAsync(int id);

    Task InserirAsync(T entity);

    Task AtualizarAsync(T entity);

    Task DeletarAsync(object key);
}
