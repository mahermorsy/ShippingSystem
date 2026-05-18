using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer.Models;
using WebApi.UI_Services.APi;

namespace WebApi.UI_Services.ShipmentServices
{
    public class ShipmentApiService : IShipmentApiService
    {
        private readonly IApiClient _apiClient;

        public ShipmentApiService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<ApiResponse<PagedResult<DtoShipment>>> GetShipmentsList(
        int pageNumber,
        int pageSize,
        bool isUserData,
        ShipmentStatusEnum? status = null)
        {
            var url = $"api/Shipments/all?Pagenumber={pageNumber}&PageSize={pageSize}&Isuserdata={isUserData}";  // ✅ أضف /all
            if (status.HasValue)
            {
                url += $"&status={(int)status.Value}";
            }

            return _apiClient.GetAsync<PagedResult<DtoShipment>>(
                url,
                true
            );
        }
        /*public Task<ApiResponse<PagedResult<DtoShipment>>> GetAllShipmentsList(int pageNumber)
         {
             return _apiClient.GetAsync<PagedResult<DtoShipment>>(
                 $"api/Shipments/all?Pagenumber={pageNumber}",
                 true
             );
         }*/
        public Task<ApiResponse<DtoShipment>> GetShipmentAsync(Guid id)
        {
            return _apiClient.GetAsync<DtoShipment>(
                $"api/Shipments/{id}",
                true
            );
        }
        public Task<ApiResponse<string>> CreateShipmentAsync(DtoShipment dto)
        {
            return _apiClient.PostAsync<DtoShipment, string>(
                "api/Shipments/Create",
                dto,
                true
            );
        }
        public Task<ApiResponse<string>> EditShipmentAsync(DtoShipment dto)
        {
            return _apiClient.PostAsync<DtoShipment, string>(
                "api/Shipments/Edit",
                dto,
                true
            );
        }
        public Task<ApiResponse<string>> DeleteShipmentAsync(Guid id)
        {
            return _apiClient.PostAsync<Guid, string>(
                "api/Shipments/Delete",
                id,
                true
            );
        }
        public Task<ApiResponse<string>> ChangeStatusAsync(DtoShipment dto)
        {
            return _apiClient.PostAsync<Guid, string>(
                $"api/Shipments/ChangeStatus?Data={(int)dto.CurrentState}",
                dto.Id,
                true
            );
        }

    }
}

