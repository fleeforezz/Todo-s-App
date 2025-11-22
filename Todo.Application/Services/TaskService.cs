using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.DTOs;
using Todo.Application.Interfaces;

namespace Todo.Application.Services
{
    public class TaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /*
         * Retrieves all tasks for a specific user.
         */
        public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId)
        {
            var tasks = await _unitOfWork.Tasks.GetTasksByUserIdAsync(userId);
            return tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                IsImportant = t.IsImportant,
                DueDate = t.DueDate,
                TagName = t.Tag?.TagName
            });
        }

        /*
         * Adds a new task to the system.
         */
        public async Task<TaskDto> AddTaskAsync(TaskDto taskDto)
        {
            var task = new Domain.Entities.Task
            {
                TaskId = Guid.NewGuid(),
                UserId = taskDto.UserId,
                TagId = taskDto.TagId ?? Guid.Empty,
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                IsCompleted = false,
                IsImportant = taskDto.IsImportant,
                IsActive = true,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            };

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            taskDto.TaskId = task.TaskId;
            return taskDto;
        }

        /*
         * Updates an existing task.
         */
        public async Task<TaskDto> UpdateTaskAsync(TaskDto taskDto)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(taskDto.TaskId);
            if (task == null)
                return null;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.IsImportant = taskDto.IsImportant;
            task.TagId = taskDto.TagId ?? Guid.Empty;
            task.UpdatedTime = DateTime.Now;

            await _unitOfWork.Tasks.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return taskDto;
        }

        /*
         * Marks a task as completed.
         */
        public async Task<bool> MarkTaskAsCompletedAsync(Guid taskId)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task == null)
                return false;
            task.IsCompleted = true;
            task.UpdatedTime = DateTime.Now;
            await _unitOfWork.Tasks.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
