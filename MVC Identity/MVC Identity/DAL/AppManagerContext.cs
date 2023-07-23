using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_First.Models;

namespace MVC_First.DAL
{
    public partial class AppManagerContext : IdentityDbContext<AppUser>
    {
        public AppManagerContext()
        {
        }
        public AppManagerContext(DbContextOptions<AppManagerContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLazyLoadingProxies(true).UseSqlServer(GetConString());

        private string GetConString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appSettings.json");
            var config = builder.Build();
            return config.GetConnectionString("DefaultConnection");
        }
        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

        public virtual DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}