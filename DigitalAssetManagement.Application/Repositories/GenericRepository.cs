using DigitalAssetManagement.Domain.Entities;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface GenericRepository<T> where T: BaseEntity
    {
        T? GetById(int id);
        Task<T?> GetByIdAsync(int id);
        ICollection<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1);
        T? GetFirstOnCondition(Func<T, bool> condition);
        Task<T?> GetFirstOnConditionAsync(Expression<Func<T, bool>> condition);
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        void BatchInsert(IEnumerable<T> entities);
        Task BatchInsertAsync(IEnumerable<T> entities);
        T Update(T entity);
        int BatchUpdate<TProperty>(Func<T, TProperty> property, Func<T, TProperty> value, Expression<Func<T, bool>>? filter = null);
        Task<int> BatchUpdateAsync<TProperty>(Func<T, TProperty> property, Func<T, TProperty> value, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null);
        T Delete(T entity);
        Task<T?> DeleteAsync(int id);
    }
}
