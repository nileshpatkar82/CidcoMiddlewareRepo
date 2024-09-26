using Microsoft.Data.SqlClient;

namespace Cidco.Middleware.Application.Contracts.Persistance
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size);
        Task<IList<T>> StoredProcedureQueryAsync(string storedProcedureName, SqlParameter[] parameters = null);
    }
}
