using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class InfoProjectViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Developer")]
        [DefaultValue("None")]
        public string? Developer { get; set; }
        [Display(Name = "Tester")]
        [DefaultValue("None")]
        public string? Tester { get; set; }

    }
}
