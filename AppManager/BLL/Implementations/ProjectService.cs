using BLL.Abstractions;
using Core.Entities;
using Core.Models;
using DAL;
using DAL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        private readonly IToDoTaskService _taskService;
        private readonly IProjectUserService _projectUserService;
        public ProjectService(UnitOfWork unitOfWork,IToDoTaskService taskService, IProjectUserService projectUserService) : base(unitOfWork)
        {
            _taskService = taskService;
            _projectUserService = projectUserService;
        }

        public async Task<bool> IfProjectNameAlreadyExists(string name, User authorizationUser)
        {
            var projects = await GetProjects(authorizationUser);
            if (projects.Any(project => project.Name == name)) { return true; }
            return false;
        }
        public override async Task<Result<bool>> DeleteItem(Guid id)
        {
            var result = await GetById(id);
            if (result.IsSuccessful && result.Data is not null)
            {
                foreach (var deletedTask in result.Data.ToDoTasks)
                {
                    await _taskService.DeleteItem(deletedTask.Id);
                }
            }
            return await base.DeleteItem(id);
        }
        public async Task<List<Project>> GetProjects(User user)
        {
            var projectIds = await _projectUserService.GetProjectsIds(user);
            var projects = new List<Project>();
            foreach (var projectId in projectIds)
            {
                var result = await GetById(projectId);
                if (result.IsSuccessful)
                {
                    projects.Add(result.Data);
                }
            }
            return projects;
        }
    }
}
