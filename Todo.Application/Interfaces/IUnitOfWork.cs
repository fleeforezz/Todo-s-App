using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        ITagRepository Tags { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
