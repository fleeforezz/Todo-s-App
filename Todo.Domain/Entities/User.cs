using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CoverImage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
