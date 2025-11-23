using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;
using Todo.Infrastructure.Persistences;

namespace Todo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoDbContext _context;
        private ITaskRepository _tasks;
        private ITagRepository _tags;
        private IUserRepository _users;

        public ITaskRepository Tasks => 
            _tasks ??= new TaskRepository(_context);

        public ITagRepository Tags => 
            _tags ??= new TagRepository(_context);

        public IUserRepository Users => 
            _users ??= new UserRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
