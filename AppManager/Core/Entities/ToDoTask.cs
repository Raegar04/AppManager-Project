using Core.Enums;
using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class ToDoTask:Entity
{

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public TaskPriority Priority { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? DeadLine { get; set; }

    public TaskCurrentStatus Status { get; set; }

    public Guid? ProjectId { get; set; }

    public virtual Project? Project { get; set; }
}
