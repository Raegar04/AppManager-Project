using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Verification code")]
        public string? Code { get; set; }
        public static string CorrectCode { get; set; }
    }
}
