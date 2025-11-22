using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Interfaces
{
    public interface IUserRepository : IRepository<Domain.Entities.User>
    {
        Task<Domain.Entities.User> GetUserByEmailAsync(string email);
        Task<Domain.Entities.User> GetUserByUserNameAsync(string userName);
    }
}
