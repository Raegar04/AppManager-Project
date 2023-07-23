using BLL.Abstractions;
using MVC_First.Models;
namespace BLL.Implementations
{
    public class NotificationService : GenericService<Notification>, INotificationService
    {
        public NotificationService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<bool> ContainsNotifications(AppUser authorizationUser, Notification newNotification)
        {
            if (authorizationUser.Notifications.Any(notification => notification.Text == newNotification.Text))
            {
                return true;
            }
            return false;
        }
        public async Task<List<Notification>> ViewNotifications(AppUser authorizationUser)
        {
            var notViewedNotifications = authorizationUser.Notifications.Where(notification => notification.IsViewed == false).ToList();
            for (int index = 0; index < notViewedNotifications.Count; index++)
            {
                notViewedNotifications[index].IsViewed = true;
            }
            return notViewedNotifications;
        }
    }
}
