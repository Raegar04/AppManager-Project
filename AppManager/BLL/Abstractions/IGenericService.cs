using Core.Entities;
using Core.Models;
using DAL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface IGenericService<CurrentEntity> where CurrentEntity : Entity
    {
        Task<Result<List<CurrentEntity>>> GetAll();
        Task<Result<bool>> AddItem(CurrentEntity item);
        Task<Result<CurrentEntity>> GetByPredicate(Func<CurrentEntity, bool> predicate);
        Task<Result<CurrentEntity>> GetById(Guid id);
        Task<Result<bool>> DeleteItem(Guid id);
        Task<Result<bool>> UpdateItem(CurrentEntity updatedItem);
    }
}
