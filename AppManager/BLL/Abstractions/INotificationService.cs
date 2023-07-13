using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface INotificationService:IGenericService<Notification>
    {
        Task<bool> ContainsNotifications(User authorizationUser, Notification newNotification);
        Task<List<Notification>> ViewNotifications(User authorizationUser);
    }
}
