using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DigitalAssetManagement.Domain.Common;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class GenericRepositoryImplementation<TEntity> : GenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepositoryImplementation(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void BatchAdd(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task BatchAddAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public int BatchUpdate(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> data = _dbSet.AsQueryable();
            
            if (filter != null)
            {
                data = data.Where(filter);
            }

            return data.ExecuteUpdate(setPropertyCalls);

        }

        public async Task<int> BatchUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default, Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> data = _dbSet.AsQueryable();

            if (filter != null)
            {
                data = data.Where(filter);
            }

            return await data.ExecuteUpdateAsync(setPropertyCalls);
        }

        public TEntity Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            return _dbSet.Remove(entity).Entity;
        }

        public async Task<TEntity?> DeleteAsync(int id)
        {
            TEntity? entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            return entity;
        }

        public bool ExistByCondition(Expression<Func<TEntity, bool>> condition)
        {
            return _dbSet.Any(condition);
        }

        public async Task<bool> ExistByConditionAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await _dbSet.AnyAsync(condition);
        }

        public ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            IQueryable<TEntity> data = _dbSet.AsQueryable();

            if (filter != null)
            {
                data = data.Where(filter);
            }

            foreach (var includeProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includeProperty);
            }
            
            if (orderedQuery != null)
            {
                data = orderedQuery(data);
            }

            if (isPaging)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            if (!isTracked)
            {
                data = data.AsNoTracking();
            }

            return data.ToList();
        }

        public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            IQueryable<TEntity> data = _dbSet.AsQueryable();

            if (filter != null)
            {
                data = data.Where(filter);
            }

            foreach (var includeProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includeProperty);
            }

            if (orderedQuery != null)
            {
                data = orderedQuery(data);
            }

            if (isPaging)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            if (!isTracked)
            {
                data = data.AsNoTracking();
            }

            return await data.ToListAsync();
        }

        public TEntity? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(int id, string includedProperties)
        {
            IQueryable<TEntity> data = _dbSet;
            foreach (var includedProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includedProperty);
            }
            return await _dbSet.FindAsync(id);
        }

        public TEntity? GetFirstOnCondition(Func<TEntity, bool> condition)
        {
            return _dbSet.FirstOrDefault(condition);
        }

        public async Task<TEntity?> GetFirstOnConditionAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await _dbSet.FirstOrDefaultAsync(condition);
        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entityEntry = await _dbSet.AddAsync(entity);
            return entityEntry.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            if (_context.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
