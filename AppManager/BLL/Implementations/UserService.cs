using BLL.Abstractions;
using Core.Entities;
using Core.Enums;
using Core.Helpers;
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
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Result<User>> Authorization(string userName, string password)
        {
            var result = await GetByPredicate((user) => user.Username == userName && user.PasswordHash == UserHelper.GetPasswordHash(password));
            if (result.Data is null || !result.IsSuccessful)
            {
                return new Result<User>(false, "Invalid username or password. '1' to get back;");
            }
            return result;
        }
        public async Task<Result<bool>> CheckUserNameIfExists(string userName)
        {
            var result = await GetAll();
            if (!result.IsSuccessful || result.Data is null)
            {
                return new Result<bool>(false, "Error in repository. Cannot find items");
            }
            if (result.Data.Any(user => user.Username == userName))
            {
                return new Result<bool>(true, true);
            }
            return new Result<bool>(true, false);
        }
        public async Task<Result<List<User>>> GetUsersByRole(UserRole role)
        {
            var result = await GetAll();
            if (!result.IsSuccessful)
            {
                return new Result<List<User>>(false, result.Message);
            }
            var users = result.Data.Where(user => user.Role == role).ToList();
            if (users is null)
            {
                return new Result<List<User>>(true, new List<User>());
            }
            return new Result<List<User>>(true, users);
        }
        
    }
}
