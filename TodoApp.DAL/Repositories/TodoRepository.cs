using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class TodoRepository : IRepository<Todo>
    {
        private TodoDbContext _db;

        public TodoRepository(TodoDbContext db)
        {
            _db = db;
        }

        /*
        *  Create 
        */
        public Todo Create(Todo todo)
        {
            _db.Todos.Add(todo);
            _db.SaveChanges();
            return todo;
        }

        /*
        *  Delete 
        */
        public bool Delete(Todo todo)
        {
            _db.Remove(todo);
            _db.SaveChanges();
            return true;
        }

        /*
        *  Delete All 
        */
        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        /*
        *  Get All 
        */
        public List<Todo> GetAll()
        {
            return _db.Todos.ToList();
        }

        /*
        *  Get By UserId 
        */
        public List<Todo> GetByUserId(Guid userId)
        {
            return _db.Todos.Where(t => t.UserId == userId).ToList();
        }

        /*
        *  Get By Id 
        */
        public Todo? GetById(Guid id)
        {
            return _db.Todos.FirstOrDefault(x => x.TodoId == id);
        }

        /*
        *  Update 
        */
        public bool Update(Todo todo)
        {
            _db = new();
            _db.Todos.Update(todo);
            _db.SaveChanges();
            return true;
        }
    }
}
