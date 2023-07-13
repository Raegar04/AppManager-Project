using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface IProjectUserService:IGenericService<ProjectUser>
    {
        Task<List<User>> GetUsers(Project project);
        Task<List<Guid>> GetProjectsIds(User user);
        Task<ProjectUser> GetByIds(Guid userId, Guid projectId);
    }
}
