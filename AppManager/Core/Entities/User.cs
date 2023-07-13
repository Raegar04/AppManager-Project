using Core.Enums;
using Core.Helpers;
using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class User:Entity
{

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
    public UserRole Role { get; set; }

    public virtual List<Notification> Notifications { get; set; } = new List<Notification>();
    //public List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    public User() { }
    public User(string username, string password, string email, string phoneNumber, UserRole role)
    {
        Username = username;
        PasswordHash = UserHelper.GetPasswordHash(password);
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
    }
}
