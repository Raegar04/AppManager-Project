using BLL.Abstractions;
using Core.Models;
using MVC_First.Models;


namespace BLL.Implementations
{
    public class ToDoTaskService : GenericService<ToDoTask>, IToDoTaskService
    {
        private readonly string directoryName = "AccompanyingDirectory";

        public ToDoTaskService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateDirectory()
        {
            DirectoryInfo AccompanyingDirectory = new DirectoryInfo(directoryName);
            if (!AccompanyingDirectory.Exists) { AccompanyingDirectory.Create(); }
        }
        public async Task<Result<bool>> AddFileToDirectory(string path)
        {
            try
            {
                string fileName = Path.GetFileName(path);
                FileInfo addingFile = new FileInfo(path);
                addingFile.CopyTo($"{directoryName}/{fileName}");
                return new Result<bool>(true);
            }
            catch
            {
                return new Result<bool>(false, "Invalid file path");
            }
        }

        public async Task<Result<ToDoTask>> GetTaskByName(string name)
        {
            var result = await _repository.GetByPredicateAsync(task => task.Name == name);
            if (!result.IsSuccessful)
            {
                return new Result<ToDoTask>(false, "Task with this name is not exists");
            }
            return result;
        }
    }
}
