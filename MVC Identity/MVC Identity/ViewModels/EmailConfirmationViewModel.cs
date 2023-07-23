using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class EmailConfirmationViewModel
    {
        [Display(Name ="Code for verification")]
        public string VerifyCode { get; set; }
        public static string CorrectCode { get; set; }
    }
}
