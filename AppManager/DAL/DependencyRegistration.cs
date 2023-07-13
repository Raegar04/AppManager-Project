using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Abstractions;
using DAL.Implementations;
using Core;

namespace DAL
{
    public class DependencyRegistration
    {
        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<UnitOfWork>();
            services.AddDbContext<AppManagerContext>();
        }
    }
}
