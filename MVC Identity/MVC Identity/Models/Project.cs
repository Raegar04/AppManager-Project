using System;
using System.Collections.Generic;

namespace MVC_First.Models;

public partial class Project
{
    public static Project currentProject;
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual List<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();
    public virtual List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
}
