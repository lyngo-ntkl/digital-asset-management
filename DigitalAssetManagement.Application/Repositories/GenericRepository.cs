using DigitalAssetManagement.Domain.Entities;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface GenericRepository<T> where T: BaseEntity
    {
        T? GetById(int id);
        Task<T?> GetByIdAsync(int id);
        ICollection<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "");
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "");
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        T Update(T entity);
        int BatchUpdate<TProperty>(Func<T, TProperty> property, Func<T, TProperty> value, Expression<Func<T, bool>>? filter = null);
        Task<int> BatchUpdateAsync<TProperty>(Func<T, TProperty> property, Func<T, TProperty> value, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null);
        T Delete(T entity);
        Task<T?> DeleteAsync(int id);
    }
}
