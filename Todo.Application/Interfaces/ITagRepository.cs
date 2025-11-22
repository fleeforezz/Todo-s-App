using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Entities;

namespace Todo.Application.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetTagsByUserIdAsync(Guid userId);
    }
}
