using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.Interfaces;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories;

namespace TodoApp.BLL.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoRepository _repo;

        public TodoService(TodoRepository repo)
        {
            _repo = repo;
        }

        /*
        *  Create Todo
        */
        public Todo CreateTodo(Todo todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Title))
            {
                throw new Exception("Todo title cannot be empty");
            }

            var newTodo = new Todo()
            {
                TodoId = Guid.NewGuid(),
                UserId = todo.UserId,
                CategoryId = todo.CategoryId,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return _repo.Create(todo);
        }

        /*
        *  Delete Todo
        */
        public bool DeleteTodo(Todo todo)
        {
            var existingTodo = _repo.GetById(todo.TodoId);
            if (existingTodo == null)
            {
                return false;
            }
            else
            {
                return _repo.Delete(existingTodo);
            } 
        }

        /*
        *  Get Todo by Id
        */
        public Todo? GetTodoById(Guid id)
        {
            return _repo.GetById(id);
        }

        /*
        *  Get Todos
        */
        public List<Todo> GetTodos(Guid userId)
        {
            return _repo.GetByUserId(userId);
        }

        /*
        *  Toggle Complete Todo
        */
        public bool ToggleCompleted(Guid todoId)
        {
            var todo = _repo.GetById(todoId);
            if (todo ==  null)
            {
                throw new Exception("Todo not found");
            }

            todo.IsCompleted = !todo.IsCompleted;
            todo.UpdatedAt = DateTime.Now;

            return _repo.Update(todo);
        }

        /*
        *  Update Todo
        */
        public bool UpdateTodo(Todo todo)
        {
            if(string.IsNullOrWhiteSpace(todo.Title))
            {
                throw new Exception("Todo title cannot be empty");
            }

            todo.UpdatedAt = DateTime.Now;

            return _repo.Update(todo);
        }
    }
}
