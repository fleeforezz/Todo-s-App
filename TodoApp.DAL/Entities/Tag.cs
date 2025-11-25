using System;
using System.Collections.Generic;

namespace TodoApp.DAL.Entities;

public partial class Tag
{
    public Guid TagId { get; set; }
    public Guid UserId { get; set; }
    public string TagName { get; set; } = null!;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
