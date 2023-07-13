using Core.Entities;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Abstractions
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<Result<IEnumerable<TEntity>>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<Result<TEntity>> GetByPredicateAsync(Func<TEntity, bool> predicate);
        Task<Result<TEntity>> GetByIdAsync(Guid id);

        Task<Result<bool>> AddItemAsync(TEntity entity);

        Task<Result<bool>> DeleteItemAsync(Guid id);


        Result<bool> UpdateItemAsync(TEntity entityToUpdate);
    }
}
