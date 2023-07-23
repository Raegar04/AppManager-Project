
using MVC_First.Models;


namespace BLL.Abstractions
{
    public interface INotificationService:IGenericService<Notification>
    {
        Task<bool> ContainsNotifications(AppUser authorizationUser, Notification newNotification);
        Task<List<Notification>> ViewNotifications(AppUser authorizationUser);
    }
}
