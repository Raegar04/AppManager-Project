using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Abstractions;
using BLL.Implementations;
using DAL;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class DependencyRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IToDoTaskService, ToDoTaskService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IProjectUserService, ProjectUserService>();

            DAL.DependencyRegistration.RegisterRepositories(services);
        }
    }
}
