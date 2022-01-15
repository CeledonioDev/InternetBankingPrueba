using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ports.Repositories
{

    /// <summary>
    /// Represents the basic operations that can be performed over a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T>
    {
        /// <summary>
        /// Stores a given <see cref="T"/>
        /// </summary>
        /// <param name="entity">An instance of <see cref="T"/></param>
        /// <returns>An implementation of <see cref="IOperationResult{T}"/></returns>
        IOperationResult<T> Create(T entity);

        /// <summary>
        /// Gets an instance of <see cref="T"/> according with the given expression parameter.
        /// </summary>
        /// <param name="predicate">Contains the filter that will be used for the search in the database.</param>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>An instance of <see cref="T"/>.</returns>
        T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets an instance of <see cref="T"/> according with the given expression parameter.
        /// </summary>
        /// <param name="predicate">Contains the filter that will be used for the search in the database.</param>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>An instance of <see cref="T"/>.</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets all the <see cref="T"/> existing.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        IEnumerable<T> Get();

        /// <summary>
        /// Gets all the <see cref="T"/> existing.
        /// </summary>
        /// <returns>An <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> GetAsync();

        /// <summary>
        /// Gets all the <see cref="T"/> existing.
        /// </summary>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>An <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> GetAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Updates a given <see cref="T"/>.
        /// </summary>
        /// <param name="entity">An instance of <see cref="T"/>.</param>
        /// <returns>An implementation of <see cref="IOperationResult{T}"/>.</returns>
        IOperationResult<T> Update(T entity);

        /// <summary>
        /// Removes a given <see cref="T"/>.
        /// </summary>
        /// <param name="entity">An instance of <see cref="T"/>.</param>
        /// <returns>An implementation of <see cref="IOperationResult{T}"/>.</returns>
        IOperationResult<T> Remove(T entity);

        /// <summary>
        /// Removes a given <see cref="T"/>.
        /// </summary>
        /// <param name="entities">An instance of <see cref="IEnumerable{T}"/>.</param>
        /// <returns>An implementation of <see cref="IOperationResult{T}"/>.</returns>
        IOperationResult<T> RemoveInRange(IEnumerable<T> entities);

        /// <summary>
        /// Gets a collection of <see cref="T"/> according with the given expression parameter.
        /// </summary>
        /// <param name="predicate">Contains the filter that will be used for the search in the database.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Performs the saving of the changes that have been executed on <see cref="T"/>.
        /// </summary>
        void Save();

        /// <summary>
        /// Checks the existence of any <see cref="T"/> that match the filter parameter.
        /// </summary>
        /// <param name="predicate">Contains the filter that will be used for the search in the database.</param>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>A <see cref="bool"/> value representing if <see cref="T"/> exists</returns>
        bool Exists(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);


        /// <summary>
        /// Checks the existence of any <see cref="T"/> that match the filter parameter.
        /// </summary>
        /// <param name="condition">Contains the filter that will be used for the search in the database.</param>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>A <see cref="bool"/> value representing if <see cref="T"/> exists</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets a collection of <see cref="T"/> according with the given expression parameter.
        /// </summary>
        /// <param name="predicate">Contains the filter that will be used for the search in the database.</param>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets a collection of <see cref="T"/> according with the given expression parameter.
        /// </summary>
        /// <param name="predicate">Contains the filter that will be used for the search in the database.</param>
        /// <param name="includes">Contains all entities related to the <see cref="T"/> that are to be included in the query.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Stores a given <see cref="T"/>
        /// </summary>
        /// <param name="entity">An instance of <see cref="T"/></param>
        /// <returns>An implementation of <see cref="IOperationResult{T}"/></returns>
        Task<IOperationResult<T>> CreateAsync(T entity);

        /// <summary>
        /// Stores a given <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="entity">An instance of <see cref="T"/></param>
        /// <returns>An implementation of <see cref="IOperationResult{IEnumerable}"/></returns>
        Task<IOperationResult<IEnumerable<T>>> CreateRangeAsync(IEnumerable<T> entity);

        /// <summary>
        /// Performs the saving of the changes that have been executed on <see cref="Task{T}"/>.
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Count the existence of any <see cref="T"/>.
        /// </summary>
        /// <returns>A  <see cref="Task{int}"/> with the number of records in <see cref="T"/>.exists</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Count the existence of any <see cref="T"/>.
        /// </summary>
        /// <see cref="Task{int}"/> with the number of records in <see cref="T"/>.
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
