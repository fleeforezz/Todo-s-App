using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;
using Todo.Infrastructure.Persistences;

namespace Todo.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TodoDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(TodoDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /*
         * Add a new entity to the database asynchronously.
         */
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /*
         * Delete an entity from the database by its unique identifier asynchronously.
         */
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        /*
         * Get all entities of type T from the database asynchronously.
         */
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /*
         * Get an entity by its unique identifier asynchronously.
         */
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /*
         * Update an existing entity in the database asynchronously.
         */
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }
    }
}
