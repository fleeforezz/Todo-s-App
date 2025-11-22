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
    public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(TodoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetImportantTasksByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.Tag)
                .Where(t => t.UserId == userId && t.IsImportant && t.IsActive)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTaskByTagAsync(Guid tagId)
        {
            return await _dbSet
                .Include(t => t.Tag)
                .Where(t => t.TagId == tagId && t.IsActive)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.Tag)
                .Where(t => t.UserId == userId && t.IsActive)
                .OrderByDescending(t => t.IsImportant)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }
    }
}
