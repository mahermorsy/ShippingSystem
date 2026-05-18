using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer.Models;
using WebApi.UI_Services.APi;

namespace WebApi.UI_Services.ShipmentServices
{
    public interface IShipmentApiService
    {
        Task<ApiResponse<string>> CreateShipmentAsync(DtoShipment dto);
        public Task<ApiResponse<string>> EditShipmentAsync(DtoShipment dto);
        Task<ApiResponse<DtoShipment>> GetShipmentAsync(Guid id);
        Task<ApiResponse<PagedResult<DtoShipment>>> GetShipmentsList(
        int pageNumber,
        int pageSize,
        bool isUserData,
        ShipmentStatusEnum? status = null);
        /*Task<ApiResponse<PagedResult<DtoShipment>>> GetAllShipmentsList(int pageNumber);*/
        Task<ApiResponse<string>> DeleteShipmentAsync(Guid id);
        Task<ApiResponse<string>> ChangeStatusAsync(DtoShipment dto);

    }
}
