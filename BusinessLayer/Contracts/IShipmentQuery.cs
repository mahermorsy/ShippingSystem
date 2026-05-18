using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer;
using DataAccessLayer.Models;
using Domains.Models;

namespace BusinessLayer.Contracts
{
    public interface IShipmentQuery : IBaseService<TbShipment, DtoShipment>
    {
        Task<DtoShipment> GetShimpentAsync(Guid ShipmentId);
        Task<DtoShipment> GetShipmentByAdminAsync(Guid ShipmentId);
        Task<PagedResult<DtoShipment>> GetShipmentsList(int pageNumber, int pageSize, bool IsUserData, ShipmentStatusEnum? Status);
    }
}
