
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class ManageActionsViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string SelectedAction { get; set; }
        [Required]
        [Display(Name = "Items")]
        public List<string> Actions { get; set; }
    }
}
