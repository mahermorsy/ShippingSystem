using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer;
using DataAccessLayer.Models;
using Domains.Models;

namespace BusinessLayer.Contracts
{
    public interface IShipmentCommand : IBaseService<TbShipment, DtoShipment>
    {
        Task<bool> CreateShipment(DtoShipment dtoShipment);
        Task<bool> EditAsync(DtoShipment dtoShipment);
        Task EditFieldsAsync(Guid ShipId, Action<TbShipment> updateAction);

    }
}
