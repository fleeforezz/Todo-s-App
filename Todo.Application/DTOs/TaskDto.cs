using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.DTOs
{
    public class TaskDto
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TagId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public DateTime DueDate { get; set; }
        public string TagName { get; set; }
    }
}
