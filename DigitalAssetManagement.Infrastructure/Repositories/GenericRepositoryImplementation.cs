using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DigitalAssetManagement.Domain.Common;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class GenericRepositoryImplementation<T> : GenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepositoryImplementation(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void BatchAdd(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task BatchAddAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public int BatchUpdate(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> data = _dbSet.AsQueryable();
            
            if (filter != null)
            {
                data = data.Where(filter);
            }

            return data.ExecuteUpdate(setPropertyCalls);

        }

        public async Task<int> BatchUpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> data = _dbSet.AsQueryable();

            if (filter != null)
            {
                data = data.Where(filter);
            }

            return await data.ExecuteUpdateAsync(setPropertyCalls);
        }

        public T Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            return _dbSet.Remove(entity).Entity;
        }

        public async Task<T?> DeleteAsync(int id)
        {
            T? entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            return entity;
        }

        public bool ExistByCondition(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Any(condition);
        }

        public async Task<bool> ExistByConditionAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.AnyAsync(condition);
        }

        public ICollection<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            IQueryable<T> data = _dbSet.AsQueryable();

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

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            IQueryable<T> data = _dbSet.AsQueryable();

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

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(int id, string includedProperties)
        {
            IQueryable<T> data = _dbSet;
            foreach (var includedProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includedProperty);
            }
            return await _dbSet.FindAsync(id);
        }

        public T? GetFirstOnCondition(Func<T, bool> condition)
        {
            return _dbSet.FirstOrDefault(condition);
        }

        public async Task<T?> GetFirstOnConditionAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.FirstOrDefaultAsync(condition);
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            var entityEntry = await _dbSet.AddAsync(entity);
            return entityEntry.Entity;
        }

        public T Update(T entity)
        {
            if (_context.Entry<T>(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
