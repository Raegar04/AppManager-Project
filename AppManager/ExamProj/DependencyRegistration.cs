using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ConsoleManagers;

namespace UI
{
    public class DependencyRegistration
    {
        public static IServiceProvider Register()
        {
            var services = new ServiceCollection();

            services.AddScoped<ProjectConsoleManager>();
            services.AddScoped<ToDoTaskConsoleManager>();
            services.AddScoped<UserConsoleManager>();
            services.AddScoped<NotificationConsoleManager>();

            BLL.DependencyRegistration.RegisterServices(services);

            return services.BuildServiceProvider();
        }
    }
}
