using BLL.Abstractions;
using Core.Entities;
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
    }
}
