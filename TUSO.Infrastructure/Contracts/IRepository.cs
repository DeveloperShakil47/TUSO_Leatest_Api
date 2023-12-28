using System.Linq.Expressions;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    /// <summary>
    /// Contains signatures of all generic methods.
    /// </summary>
    /// <typeparam name="T">T is a model class.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Creates a row in the table.
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        T Add(T entity);

        /// <summary>
        /// add multi object to database
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<T> entities);
        /// <summary>
        /// Returns a row from the table as an object if primary key matches.
        /// </summary>
        /// <param name="id">Primary key Guid</param>
        /// <returns>Retrieved object.</returns>
        T Get(Guid id);

        /// <summary>
        /// Returns a row from the table as an object if primary key matches.
        /// </summary>
        /// <param name="id">Primary key int</param>
        /// <returns>Retrieved object.</returns>
        T Get(int id);

        /// <summary>
        /// Returns all rows as a list of objects from the table.
        /// </summary>
        /// <returns>List of objects.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Returns matched rows as a list of objects.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression.</param>
        /// <returns>List of objects.</returns>
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order);

        /// <summary>
        /// The method is used to get a list of data.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression.</param>
        /// <returns>Returns matched rows as a list of data.</returns>
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns matched rows as a list of objects.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression in where clause.</param>
        /// <param name="obj">Custom parameter indicate to call get using include</param>
        /// <returns>List of objects.</returns>
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Expression<Func<T, object>> obj);

        /// <summary>
        /// Returns matched rows as a list of objects.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression.</param>
        /// <param name="obj">Custom parameter indicate to call get using include</param>
        /// <param name="next">Custom parameter indicate to call get using again include</param>
        /// <returns>List of objects.</returns>
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Expression<Func<T, object>> obj, Expression<Func<T, object>> next);

        /// <summary>
        /// Returns first matched row as an object from the table.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression.</param>
        /// <returns>Retrieved object.</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<T> LoadWithChildAsync<TEntity>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] expressionList);

        Task<IEnumerable<T>> LoadListWithChildAsync<TEntity>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] expressionList);

        Task<IEnumerable<T>> LoadListWithChildAsync<TEntity>(Expression<Func<T, bool>> predicate, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] expressionList);

        /// <summary>
        /// Returns first matched row as an object from the table.
        /// </summary>
        /// <param name="predicate">Custom LINQ expression.</param>
        /// <param name="obj">Custom parameter indicate to call get using include</param>
        /// <returns>Retrieved object.</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> obj);

        /// <summary>
        /// Updates an existing row in the table.
        /// </summary>
        /// <param name="entity">Object to be updated.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes a row from the table.
        /// </summary>
        /// <param name="entity">Object to be deleted.</param>
        void Delete(T entity);
    }
}