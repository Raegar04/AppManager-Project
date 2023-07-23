
using MVC_First.Models;


namespace BLL.Abstractions
{
    public interface IProjectUserService:IGenericService<ProjectUser>
    {
        Task<ProjectUser> GetByIds(string userId, string projectId);
    }
}
