using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface IProjectService:IGenericService<Project>
    {
        Task<bool> IfProjectNameAlreadyExists(string name, User authorizationUser);
        Task<List<Project>> GetProjects(User user);
    }
}
