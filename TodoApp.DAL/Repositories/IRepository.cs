using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public interface IRepository<T>
    {
        T GetById(Guid id);
        List<T> GetAll();
        T Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        void DeleteAll();
    }
}
