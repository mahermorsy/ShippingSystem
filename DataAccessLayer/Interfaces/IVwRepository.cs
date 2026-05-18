
using Domains.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IVwRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> Filter);
        Task<List<T>> GetList(Expression<Func<T, bool>> Filter);
    }
}
