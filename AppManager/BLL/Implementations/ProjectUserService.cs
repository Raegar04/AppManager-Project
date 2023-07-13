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
    public class ProjectUserService : GenericService<ProjectUser>, IProjectUserService
    {

        public ProjectUserService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<User>> GetUsers(Project project)
        {
            var users = new List<User>();
            var projectUsers = await GetAll();
            if (!projectUsers.IsSuccessful)
            {
                return users;
            }
            foreach (var item in projectUsers.Data)
            {
                if (item.ProjectId == project.Id)
                {
                    users.Add(item.User);
                }
            }
            return users;
        }
        public async Task<List<Guid>> GetProjectsIds(User user)
        {
            var projectIds = new List<Guid>();
            var projectUsers = await GetAll();
            if (!projectUsers.IsSuccessful)
            {
                return projectIds;
            }
            foreach (var item in projectUsers.Data)
            {
                if (item.UserId == user.Id)
                {
                    projectIds.Add(item.ProjectId);
                }
            }
            return projectIds;
        }
        public async Task<ProjectUser> GetByIds(Guid userId, Guid projectId)
        {
            var result = await GetByPredicate(item => item.UserId == userId && item.ProjectId == projectId);
            return result.Data;
        }
    }
}
