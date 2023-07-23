using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_First.Models;

public partial class ToDoTask 
{
    public static ToDoTask toDoTask;
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public TaskPriority Priority { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime DeadLine { get; set; }

    public TaskCurrentStatus Status { get; set; }

    [ForeignKey("Project")]
    public string? ProjectId { get; set; }

    public virtual Project? Project { get; set; }
}
