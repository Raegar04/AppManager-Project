
using Core.Models;
using MVC_First.Models;


namespace BLL.Abstractions
{
    public interface IProjectService:IGenericService<Project>
    {
        Task<bool> IfProjectNameAlreadyExists(string name, AppUser authorizationUser);
        Task<Result<Project>> GetProjectByName(string name);
    }
}
