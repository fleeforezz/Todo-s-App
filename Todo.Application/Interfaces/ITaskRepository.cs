using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Interfaces
{
    public interface ITaskRepository : IRepository<Domain.Entities.Task>
    {
        Task<IEnumerable<Domain.Entities.Task>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<Domain.Entities.Task>> GetImportantTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<Domain.Entities.Task>> GetTaskByTagAsync(Guid tagId);
    }
}
