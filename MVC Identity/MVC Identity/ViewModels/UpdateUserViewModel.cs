using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = $"Value is too short",MinimumLength =5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
