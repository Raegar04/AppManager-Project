using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Notification:Entity
{ 

    public string Text { get; set; } = null!;

    public bool IsViewed { get; set; }

    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
}
