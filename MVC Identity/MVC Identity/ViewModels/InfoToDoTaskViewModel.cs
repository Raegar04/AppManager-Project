using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class InfoToDoTaskViewModel
    {
        [Required]
        [Display(Name ="Title")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;
        [Required]
        [Display(Name = "Priority")]
        public TaskPriority Priority { get; set; }
        [Required]
        [Display(Name = "StartTime")]
        public DateTime StartTime { get; set; }
        [Required]
        [Display(Name = "DeadLine")]
        public DateTime DeadLine { get; set; }
        public TaskCurrentStatus Status { get; set; }
    }
}
