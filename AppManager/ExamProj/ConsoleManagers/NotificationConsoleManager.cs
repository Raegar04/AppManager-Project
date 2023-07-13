using BLL.Abstractions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ConsoleManagers
{
    internal class NotificationConsoleManager : BaseConsoleManager<INotificationService, Notification>
    {
        private User authorizationUser;
        private readonly IProjectService projectService;
        public NotificationConsoleManager(INotificationService service, IProjectService projectService) : base(service)
        {
            this.projectService = projectService;
        }

        internal async Task<User> StartProcessAsync(User newUser)
        {
            authorizationUser = newUser;
            var projects = await projectService.GetProjects(authorizationUser);
            foreach (var project in projects)
            {
                foreach (var task in project.ToDoTasks)
                {
                    if (task.DeadLine < DateTime.Now)
                    {
                        var newNotification = new Notification() { Text = $"You`ve missed deadline of the task '{task.Name}'. It was {task.DeadLine}" };
                        if (!await _service.ContainsNotifications(authorizationUser, newNotification))
                        {
                            authorizationUser.Notifications.Add(newNotification);
                        }
                    }
                }
            }
            await ViewNotificationsAsync();

            return authorizationUser;
        }
        private async Task ViewNotificationsAsync()
        {
            var notViewedNotifications = await _service.ViewNotifications(authorizationUser);
            if (notViewedNotifications.Count > 0)
            {
                Console.WriteLine($"You have {notViewedNotifications.Count} not viewed notifications:");
                for (int index = 0; index < notViewedNotifications.Count; index++)
                {
                    await PrintNotificationAsync(notViewedNotifications[index]);
                }
            }
            else
            {
                Console.WriteLine($"You haven`t got any not viewed notifications.");
            }
        }
        private async Task PrintNotificationAsync(Notification notification)
        {
            Console.WriteLine(notification.Text);
        }
        internal async Task PrintAllNotificationsAsync()
        {
            foreach (var notification in authorizationUser.Notifications)
            {
                await PrintNotificationAsync(notification);
            }
        }
    }
}
