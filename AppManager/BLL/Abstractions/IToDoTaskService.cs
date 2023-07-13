using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface IToDoTaskService : IGenericService<ToDoTask>
    {
        //Task CreateDirectory();
        //Task<Result<bool>> AddFileToDirectory(string path);
    }
}
