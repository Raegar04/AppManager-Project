using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_First.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class DetailUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        public string Role { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();

    }
}
