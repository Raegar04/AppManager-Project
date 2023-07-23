using System;
using System.Collections.Generic;

namespace MVC_First.Models;

public partial class ProjectUser 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProjectId { get; set; }

    public string UserId { get; set; }

    public virtual Project? Project { get; set; }

    public virtual AppUser? User { get; set; }
}
