using BLL.Abstractions;
using MVC_First.Models;
namespace BLL.Implementations
{
    public class ProjectUserService : GenericService<ProjectUser>, IProjectUserService
    {

        public ProjectUserService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ProjectUser> GetByIds(string userId, string projectId)
        {
            var result = await GetByPredicate(item => item.UserId == userId && item.ProjectId == projectId);
            return result.Data;
        }
    }
}
