using Core;
using Core.Entities;
using DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork:IDisposable
    {
        private AppManagerContext _appContext = new AppManagerContext();
        private Repository<User>? userRepos;
        private Repository<Project>? projectRepos;
        private Repository<ProjectUser>? projectUserRepos;
        private Repository<Notification>? notifyRepos;
        private Repository<ToDoTask>? taskRepos;
        public Repository<User> UserRepository 
        {
            get 
            {
                if (userRepos == null) { userRepos = new Repository<User>(_appContext); }
                return userRepos;
            }
        }
        public Repository<Project> ProjectRepository
        {
            get
            {
                if (projectRepos == null) { projectRepos = new Repository<Project>(_appContext); }
                return projectRepos;
            }
        }
        public Repository<ProjectUser> ProjectUserRepository
        {
            get
            {
                if (projectUserRepos == null) { projectUserRepos = new Repository<ProjectUser>(_appContext); }
                return projectUserRepos;
            }
        }
        public Repository<ToDoTask> TaskRepository
        {
            get
            {
                if (taskRepos == null) { taskRepos = new Repository<ToDoTask>(_appContext); }
                return taskRepos;
            }
        }
        public Repository<Notification> NotificationRepository
        {
            get
            {
                if (notifyRepos == null) { notifyRepos = new Repository<Notification>(_appContext); }
                return notifyRepos;
            }
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _appContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
