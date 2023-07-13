using BLL.Abstractions;
using Core.Entities;
using Core.Models;
using DAL;
using DAL.Abstractions;
using DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public abstract class GenericService<CurrentEntity> : IGenericService<CurrentEntity> where CurrentEntity : Entity
    {
        protected readonly IRepository<CurrentEntity> _repository;
        public GenericService(UnitOfWork unitOfWork)
        {
            if (typeof(CurrentEntity)==typeof(User))
            {
                _repository = (IRepository<CurrentEntity>?)unitOfWork.UserRepository;
            }
            else if (typeof(CurrentEntity) == typeof(Project))
            {
                _repository = (IRepository<CurrentEntity>?)unitOfWork.ProjectRepository;
            }
            else if (typeof(CurrentEntity) == typeof(ToDoTask))
            {
                _repository = (IRepository<CurrentEntity>?)unitOfWork.TaskRepository;
            }
            else if (typeof(CurrentEntity) == typeof(Notification))
            {
                _repository = (IRepository<CurrentEntity>?)unitOfWork.NotificationRepository;
            }
            if (typeof(CurrentEntity) == typeof(ProjectUser))
            {
                _repository = (IRepository<CurrentEntity>?)unitOfWork.ProjectUserRepository;
            }
        }
        public virtual async Task<Result<bool>> AddItem(CurrentEntity item)
        {
            return await _repository.AddItemAsync(item);
        }

        public virtual async Task<Result<bool>> DeleteItem(Guid id)
        {
            return await _repository.DeleteItemAsync(id);
        }

        public virtual async Task<Result<List<CurrentEntity>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            if (result.Data is null)
            {
                return new Result<List<CurrentEntity>>(false, "Cannot find items");
            }
            return new Result<List<CurrentEntity>>(true, result.Data.ToList());
        }

        public virtual async Task<Result<CurrentEntity>> GetById(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result.Data is null)
            {
                return new Result<CurrentEntity>(false, "Cannot find item");
            }
            return result;
        }

        public virtual async Task<Result<CurrentEntity>> GetByPredicate(Func<CurrentEntity, bool> predicate)
        {
            var result = await _repository.GetByPredicateAsync(predicate);
            if (result.Data is null)
            {
                return new Result<CurrentEntity>(false, "Cannot find item");
            }
            return result;
        }

        public virtual async Task<Result<bool>> UpdateItem(CurrentEntity updatedItem)
        {
            return _repository.UpdateItemAsync(updatedItem);
        }
    }
}
