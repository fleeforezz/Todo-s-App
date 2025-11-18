using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.BLL.Interfaces
{
    public interface ITodoService
    {
        Todo CreateTodo(Todo todo);
        List<Todo> GetTodos(Guid userId);
        Todo GetTodoById(Guid id);
        bool UpdateTodo(Todo todo);
        bool DeleteTodo(Todo todo);
        bool ToggleCompleted(Guid todoId);
    }
}
