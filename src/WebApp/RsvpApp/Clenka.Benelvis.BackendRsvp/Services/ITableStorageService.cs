using Azure.Data.Tables;
using System.Linq.Expressions;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public interface ITableStorageService<T> where T : class, ITableEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(string id);
        Task<Azure.Response> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteASync(string id);
    }
}
