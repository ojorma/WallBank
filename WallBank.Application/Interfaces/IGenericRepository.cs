
using System.Linq.Expressions;

namespace WallBank.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<T> AddAsync(T entity);
        Task<int> AddManyAsync(List<T> entities);
        Task<int> DeleteManyAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task<int> UpdateRangeAsync(List<T> entities);
        Task DeleteAsync(T entity);

        Task<IQueryable<T>> GetAllQueryable(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> GetPaginatedReponseAsync(IQueryable<T> query, int pageNumber, int pageSize);
        Task<IQueryable<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize);
        Task<T?> GetBy(Expression<Func<T, bool>> predicate);
        Task<int> Count(Expression<Func<T, bool>> predicate);

        void ClearChanges();
      
    }
}
