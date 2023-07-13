using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class ProjectUser:Entity
{
    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public virtual Project? Project { get; set; }

    public virtual User? User { get; set; }
}
