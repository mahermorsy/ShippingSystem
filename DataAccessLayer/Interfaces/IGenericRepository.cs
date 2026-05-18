
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<bool> AddAsync(T entity, Guid USerID);
        Task<(bool Success, Guid EntityId)> AddAsyncGetID(T entity, Guid userId);
        Task UpdateAsync(T entity, Guid USerID);
        Task DeleteAsync(Guid id);
        Task<int> ChangeStatus(Guid id , Guid UserId, int Status=1);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> Filter);  
        Task<List<T>> GetList(Expression<Func<T, bool>> Filter);
        Task<List<Tresult>> GetList<Tresult>(
           Expression<Func<T, bool>> Filter,
           Expression<Func<T, Tresult>>? Selector,
           Expression<Func<T, object>>? OrderBy ,
           bool IsDescending = false,
           params Expression<Func<T, object>>[] includes);
        Task<PagedResult<Tresult>> GetPagedList<Tresult>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, Tresult>> selector,
            int pageNumber = 1,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false,
            params Expression<Func<T, object>>[] includes);
        Task<Tresult> GetByIdAsync<Tresult>(
             Expression<Func<T, bool>> filter,
             Expression<Func<T, Tresult>> selector = null,
             Expression<Func<T, object>> orderBy = null,
             bool isDescending = false,
             params Expression<Func<T, object>>[] includes);
        Task<bool> UpdateFieldsync(Guid id, Action<T> updateAction);
    }



}
