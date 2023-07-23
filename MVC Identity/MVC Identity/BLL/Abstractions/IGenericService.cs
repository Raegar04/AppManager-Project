
using Core.Models;

namespace BLL.Abstractions
{
    public interface IGenericService<CurrentEntity> where CurrentEntity : class
    {
        Task<Result<List<CurrentEntity>>> GetAll();
        Task<Result<bool>> AddItem(CurrentEntity item);
        Task<Result<CurrentEntity>> GetByPredicate(Func<CurrentEntity, bool> predicate);
        //Task<Result<CurrentEntity>> GetById(Guid id);
        Task<Result<bool>> DeleteItem(string id);
        Task<Result<bool>> UpdateItem(CurrentEntity updatedItem);
    }
}
