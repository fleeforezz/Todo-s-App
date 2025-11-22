using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Entities
{
    public class Tag
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
