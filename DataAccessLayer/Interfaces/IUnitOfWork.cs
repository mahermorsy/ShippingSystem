using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Domains.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseTable;
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();

    }
}




