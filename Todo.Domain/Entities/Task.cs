using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Entities
{
    public class Task
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public Guid TagId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; }
        public Tag Tag { get; set; }
    }
}
