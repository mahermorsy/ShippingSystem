using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface IBaseService<T,DTO>
    {
        Task<List<DTO>>GetAllAsync();
        Task<DTO> GetByIdAsync(Guid id);
        Task<bool> AddAsync(DTO entity);
        Task<bool> AddAsync(DTO entity, Guid auditUserId);
        Task<(bool Success, Guid EntityId)> AddAsyncWithID(DTO entity);
        Task<(bool Success, Guid EntityId)> AddAsyncWithID(DTO entity, Guid auditUserId);
        Task UpdateAsync(DTO entity);
        Task<bool> ChangeStatus(Guid id, int Status = 1);
    }
}
