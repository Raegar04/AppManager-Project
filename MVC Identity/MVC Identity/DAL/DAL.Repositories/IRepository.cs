
using Core.Models;
using MVC_First.Models;
using System.Linq.Expressions;

namespace MVC_First.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<Result<IEnumerable<TEntity>>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<Result<TEntity>> GetByPredicateAsync(Func<TEntity, bool> predicate);
        //Task<Result<TEntity>> GetByIdAsync(Guid id);

        Task<Result<bool>> AddItemAsync(TEntity entity);

        Task<Result<bool>> DeleteItemAsync(string id);


        Result<bool> UpdateItemAsync(TEntity entityToUpdate);
    }
}
