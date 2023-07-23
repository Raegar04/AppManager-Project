using MVC_First.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.ViewModels
{
    public class ManageEntitiesViewModel<Entity> 
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Action { get; set; }
        [Display(Name = "Items")]
        public List<Entity> Items { get; set; } = new List<Entity>();
    }
}
