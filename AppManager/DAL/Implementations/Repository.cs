using Core;
using Core.Entities;
using Core.Models;
using DAL.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public AppManagerContext context;
        public DbSet<TEntity> dbSet;

        public Repository(AppManagerContext appManagerContext)
        {
            context = appManagerContext;
            dbSet = context.Set<TEntity>();
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                             (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return new Result<IEnumerable<TEntity>>(true, await orderBy(query).ToListAsync());
                }
                else
                {
                    return new Result<IEnumerable<TEntity>>(true, await query.ToListAsync());
                }
            }
            catch
            {
                return new Result<IEnumerable<TEntity>>(false, "Cannot get item");
            }

        }
        public async Task<Result<TEntity>> GetByPredicateAsync(Func<TEntity, bool> predicate)
        {
            try
            {
                var result = await GetAllAsync();
                if (!result.IsSuccessful)
                {
                    return new Result<TEntity>(false, result.Message);
                }
                return new Result<TEntity>(true, result.Data.First(predicate));
            }
            catch (Exception e)
            {
                return new Result<TEntity>(false, $"Cannot find item.{e.Message}");
            }
        }
        public virtual async Task<Result<TEntity>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await GetByPredicateAsync(item => item.Id == id);
                return new Result<TEntity>(true, result.Data);
            }
            catch
            {
                return new Result<TEntity>(false, "Cannot find items");
            }
        }

        public virtual async Task<Result<bool>> AddItemAsync(TEntity entity)
        {

            try
            {
                await dbSet.AddAsync(entity);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch
            {
                //UpdateItemAsync(entity);
                return new Result<bool>(false, "Cannot add item");
            }
        }

        public async Task<Result<bool>> DeleteItemAsync(Guid id)
        {
            try
            {
                var entityToDelete = await dbSet.FindAsync(id);
                Delete(entityToDelete);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (NullReferenceException)
            {
                return new Result<bool>(false, "Cannot find the item with this id");
            }
            catch
            {
                return new Result<bool>(false, "Cannot delete the item");
            }
        }

        private void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual Result<bool> UpdateItemAsync(TEntity entityToUpdate)
        {
            try
            {
                dbSet.Attach(entityToUpdate);
                context.Entry(entityToUpdate).State = EntityState.Modified;
                context.SaveChanges();
                return new Result<bool>(true);
            }
            catch
            {
                return new Result<bool>(false, "Cannot update item");
            }
        }
    }
}
