using DigitalAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface GenericRepository<T> where T: BaseEntity
    {
        T? GetById(int id);
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(int id, string includedProperties);
        ICollection<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1);
        T? GetFirstOnCondition(Func<T, bool> condition);
        Task<T?> GetFirstOnConditionAsync(Expression<Func<T, bool>> condition);
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        void BatchInsert(IEnumerable<T> entities);
        Task BatchInsertAsync(IEnumerable<T> entities);
        T Update(T entity);
        int BatchUpdate(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>>? filter = null);
        Task<int> BatchUpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null);
        T Delete(T entity);
        Task<T?> DeleteAsync(int id);
    }
}
