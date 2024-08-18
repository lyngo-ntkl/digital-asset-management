using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public int BatchUpdate<TProperty>(Func<T, TProperty> property, Func<T, TProperty> value, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> data = _dbSet.AsQueryable();
            
            if (filter != null)
            {
                data = data.Where(filter);
            }

            return data.ExecuteUpdate(data => data.SetProperty(property, value));

        }

        public async Task<int> BatchUpdateAsync<TProperty>(Func<T, TProperty> property, Func<T, TProperty> value, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> data = _dbSet.AsQueryable();

            if (filter != null)
            {
                data = data.Where(filter);
            }

            return await data.ExecuteUpdateAsync(data => data.SetProperty(property, value));
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

        public ICollection<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "")
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
                return orderedQuery(data).ToList();
            }

            return data.ToList();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null, string includedProperties = "")
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
                return await orderedQuery(data).ToListAsync();
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

        public T Insert(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task<T> InsertAsync(T entity)
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
