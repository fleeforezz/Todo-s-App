using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.DAL.Entities;

public partial class Todo
{
    public Guid TodoId { get; set; }
    public Guid UserId { get; set; }
    public Guid? TagId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; } = null;
    public DateTime? DueDate { get; set; }
    public bool? IsCompleted { get; set; } = false;
    public bool? IsImportant { get; set; } = false;
    public bool? IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Tag? Tag { get; set; }
    public virtual User User { get; set; } = null!;

    // Not mapped property for CategoryName
    [NotMapped]
    public string TagName { get; set; } = null!;
}
