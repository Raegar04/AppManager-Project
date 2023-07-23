using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_First.Models;

public partial class Notification 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; } = null!;

    public bool IsViewed { get; set; }
    [ForeignKey("AppUser")]
    public string? UserId { get; set; }
    public virtual AppUser? User { get; set; }
}
