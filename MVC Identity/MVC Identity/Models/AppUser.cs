using Microsoft.AspNetCore.Identity;
using MVC_First.Models;

public class AppUser : IdentityUser
{
    //public Guid Id { get; set; } = Guid.NewGuid();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
}

