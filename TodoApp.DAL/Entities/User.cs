using System;
using System.Collections.Generic;

namespace TodoApp.DAL.Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
