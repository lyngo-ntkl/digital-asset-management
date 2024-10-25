using DigitalAssetManagement.Domain.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface GenericRepository<TEntity> where TEntity: BaseEntity
    {
        bool ExistByCondition(Expression<Func<TEntity, bool>> condition);
        Task<bool> ExistByConditionAsync(Expression<Func<TEntity, bool>> condition);
        IEnumerable<TProperty> GetPropertyValue<TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, Expression<Func<TEntity, bool>>? filter = null);
        TEntity? GetById(int id);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(int id, string includedProperties);
        ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1);
        Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1);
        TEntity? GetFirstOnCondition(Func<TEntity, bool> condition);
        Task<TEntity?> GetFirstOnConditionAsync(Expression<Func<TEntity, bool>> condition);
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        void BatchAdd(IEnumerable<TEntity> entities);
        Task BatchAddAsync(IEnumerable<TEntity> entities);
        void BatchDelete(Expression<Func<TEntity, bool>>? filter = null);
        Task BatchDeleteAsync(Expression<Func<TEntity, bool>>? filter = null);
        TEntity Update(TEntity entity);
        int BatchUpdate(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null);
        Task<int> BatchUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
        TEntity Delete(TEntity entity);
        Task<TEntity?> DeleteAsync(int id);
    }
}
