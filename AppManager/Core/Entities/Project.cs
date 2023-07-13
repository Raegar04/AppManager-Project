using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Project:Entity
{

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual List<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();
    //public List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
}
