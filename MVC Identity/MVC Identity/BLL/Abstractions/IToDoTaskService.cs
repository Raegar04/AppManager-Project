
using Core.Models;
using MVC_First.Models;

namespace BLL.Abstractions
{
    public interface IToDoTaskService : IGenericService<ToDoTask>
    {
        //Task CreateDirectory();
        //Task<Result<bool>> AddFileToDirectory(string path);
        Task<Result<ToDoTask>> GetTaskByName(string name);
    }
}
