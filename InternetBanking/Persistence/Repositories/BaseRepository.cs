using Core.Contracts;
using Core.Models;
using Core.Ports.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IGenericRepository<T>
        where T : class, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _set;
        public BaseRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        IOperationResult<T> IGenericRepository<T>.Create(T entity)
        {
            _context.Add(entity);
            return BasicOperationResult<T>.Ok(entity);
        }

        T IGenericRepository<T>.Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable.FirstOrDefault(predicate);
        }

        async Task<T> IGenericRepository<T>.FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return await queryable.FirstOrDefaultAsync(predicate);
        }

        public IOperationResult<T> RemoveInRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);

            return BasicOperationResult<T>.Ok();
        }

        IEnumerable<T> IGenericRepository<T>.FindAll(Expression<Func<T, bool>> predicate) => _set.Where(predicate).ToList();

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            return await queryable.AnyAsync(condition);
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<T, bool>> condition,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable.AnyAsync(condition);
        }

        public virtual Task SaveAsync() => _context.SaveChangesAsync();

        IEnumerable<T> IGenericRepository<T>.Get()
        => _set.AsEnumerable();

        async Task<IEnumerable<T>> IGenericRepository<T>.GetAsync()
        => await _set.AsQueryable().ToListAsync();

        async Task<IEnumerable<T>> IGenericRepository<T>.GetAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return await queryable.ToListAsync();
        }

        IOperationResult<T> IGenericRepository<T>.Remove(T entity)
        {
            _context.Remove(entity);

            return BasicOperationResult<T>.Ok();
        }

        void IGenericRepository<T>.Save()
            => _context.SaveChanges();

        bool IGenericRepository<T>.Exists(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable.Any(predicate);
        }

        public virtual async Task<IOperationResult<T>> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return BasicOperationResult<T>.Ok(entity);
        }

        public virtual async Task<IOperationResult<IEnumerable<T>>> CreateRangeAsync(IEnumerable<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);

            return BasicOperationResult<IEnumerable<T>>.Ok(entity);
        }

        IOperationResult<T> IGenericRepository<T>.Update(T entity)
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;

            return BasicOperationResult<T>.Ok();
        }

        IEnumerable<T> IGenericRepository<T>.FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable.Where(predicate).ToList();
        }

        async Task<IEnumerable<T>> IGenericRepository<T>.FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return await queryable.Where(predicate).ToListAsync();
        }

        async Task<int> IGenericRepository<T>.CountAsync()
        {
            IQueryable<T> queryable = _set.AsQueryable();

            return await queryable.CountAsync();
        }

        async Task<int> IGenericRepository<T>.CountAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            return await queryable.Where(predicate).CountAsync();
        }
    }
}
