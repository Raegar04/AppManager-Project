using Core.Entities;
using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface IUserService:IGenericService<User>
    {
        Task<Result<User>> Authorization(string userName, string password);
        Task<Result<bool>> CheckUserNameIfExists(string userName);
        Task<Result<List<User>>> GetUsersByRole(UserRole role);
    }
}
