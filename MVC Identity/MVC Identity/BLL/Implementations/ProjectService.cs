using BLL.Abstractions;
using Core.Models;
using MVC_First.Models;

namespace BLL.Implementations
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        private readonly IToDoTaskService _taskService;
        public ProjectService(UnitOfWork unitOfWork, IToDoTaskService taskService) : base(unitOfWork)
        {
            _taskService = taskService;
        }

        public async Task<bool> IfProjectNameAlreadyExists(string name, AppUser authorizationUser)
        {
            //var projects = await GetProjects(authorizationUser);
            //if (projects.Any(project => project.Name == name)) { return true; }
            return false;
        }
        public override async Task<Result<bool>> DeleteItem(string id)
        {
            var result = await GetByPredicate(item=> item.Id==id);
            if (result.IsSuccessful && result.Data is not null)
            {
                foreach (var deletedTask in result.Data.ToDoTasks)
                {
                    await _taskService.DeleteItem(deletedTask.Id);
                }
            }
            return await base.DeleteItem(id);
        }

        public async Task<Result<Project>> GetProjectByName(string name)
        {
            var result = await _repository.GetByPredicateAsync(proj=>proj.Name==name);
            if (!result.IsSuccessful)
            {
                return new Result<Project>(false, "Project with this name is not exists");
            }
            return result;
        }
    }
}
