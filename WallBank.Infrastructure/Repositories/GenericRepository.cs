
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Linq.Expressions;
using WallBank.Application.Interfaces;
using WallBank.Infrastructure.Context;

namespace WallBank.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDataContext _dbContext;

        public GenericRepository(ApplicationDataContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.Clear();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public Task<IQueryable<T>> GetAllQueryable(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsNoTracking();


            if (filter != null)
            {
                query = query.Where(filter);
            }
            //  query =query.Where(c=>c.GetType() == typeof(T).);
            //    var query1 = _dbContext.SoundEx("jhkjh");
            var count = 0;
            if (includes != null)
            {
                while (count < includes.Length)
                {
                    query.Include(includes[count]);
                    count++;
                }
            }
            if (orderBy != null)
            {
                //     query = query.OrderBy(c => c.Equals(orderBy));
            }

            return Task.FromResult(query);
        }




        public async Task<IReadOnlyList<T>> GetPaginatedReponseAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            return await
                query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<IQueryable<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            query =
                query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .AsQueryable();
            return Task.FromResult(query);
        }

        public async Task<T?> GetBy(Expression<Func<T, bool>> predicate)
        {
            var result = await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
            return result;
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().CountAsync(predicate);
        }

        public async Task<int> AddManyAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
           
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteManyAsync(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(List<T> entities)
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.Set<T>().UpdateRange(entities);
            return await _dbContext.SaveChangesAsync();

        }

        public void ClearChanges()
        {
            _dbContext.ChangeTracker.Clear();
        }

       

     
    }
}
