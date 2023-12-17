using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IRepository interface.
    /// </summary>
    /// <typeparam name="T">T is a model class.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }

        public virtual T Add(T entity)
        {
            try
            {
                return context.Set<T>().Add(entity).Entity;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add multi object to database
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
        }
        public virtual T Get(Guid id)
        {
            try
            {
                return context.Set<T>().Find(id);
            }
            catch
            {
                throw;
            }
        }

        public virtual T Get(int id)
        {
            try
            {
                return context.Set<T>().Find(id);
            }
            catch
            {
                throw;
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return context.Set<T>()
                    .AsQueryable()
                    .AsNoTracking()
                    .ToList();
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order)
        {
            try
            {
                return await context.Set<T>()
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderByDescending(order)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns matched rows as a list of objects.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression.</param>
        /// <returns>List of objects.</returns>
        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await context.Set<T>()
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(predicate)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Expression<Func<T, object>> obj)
        {
            try
            {
                return await context.Set<T>()
                    .AsQueryable()
                    .Where(predicate)
                    .OrderByDescending(order)
                    .Include(obj)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Expression<Func<T, object>> obj, Expression<Func<T, object>> next)
        {
            try
            {
                return await context.Set<T>()
                    .AsQueryable()
                    .Where(predicate)
                    .OrderByDescending(order)
                    .Include(obj)
                    .Include(next)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Expression<Func<T, object>> obj, Expression<Func<T, object>> next, Expression<Func<T, object>> obj2)
        {
            try
            {
                return await context.Set<T>()
                      .AsQueryable()
                      .Where(predicate)
                      .OrderByDescending(order)
                      .Include(obj)
                      .Include(next)
                      .Include(obj2)
                      .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await context.Set<T>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> obj)
        {
            try
            {
                return await context.Set<T>()
                    .AsNoTracking()
                    .Include(obj)
                    .FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> obj, Expression<Func<T, object>> next)
        {
            try
            {
                return await context.Set<T>()
                    .AsNoTracking()
                    .Include(obj)
                    .Include(next)
                    .FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                context.Set<T>().Update(entity);
            }
            catch
            {
                throw;
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
            }
            catch
            {
                throw;
            }
        }
    }
}