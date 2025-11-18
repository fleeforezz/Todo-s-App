using System;
using System.Collections.Generic;

namespace TodoApp.DAL.Entities;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();

    public virtual User User { get; set; } = null!;
}
