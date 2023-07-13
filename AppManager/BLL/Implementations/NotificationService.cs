using BLL.Abstractions;
using Core.Entities;
using DAL;
using DAL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class NotificationService : GenericService<Notification>, INotificationService
    {
        public NotificationService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<bool> ContainsNotifications(User authorizationUser, Notification newNotification)
        {
            if (authorizationUser.Notifications.Any(notification => notification.Text == newNotification.Text))
            {
                return true;
            }
            return false;
        }
        public async Task<List<Notification>> ViewNotifications(User authorizationUser)
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
