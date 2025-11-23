using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;
using Todo.Infrastructure.Persistences;

namespace Todo.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(TodoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> GetTagsByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(tag => tag.User.UserId == userId)
                .OrderBy(tag => tag.TagName)
                .ToListAsync();
        }
    }
}
