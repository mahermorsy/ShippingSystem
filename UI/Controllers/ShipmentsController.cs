using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.UI_Services.APi;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentQuery _shipmentService;
        private readonly IShipmentCommand _shipmentCommand;
        IShipmentStatusHandlerFactory _IshipmentStatusHandlerFactory;

        public ShipmentsController(IShipmentQuery shipmentService, IShipmentCommand shipmentCommand, IShipmentStatusHandlerFactory shipmentStatusHandlerFactory)
        {
            _shipmentService = shipmentService;
            _shipmentCommand = shipmentCommand;
            _IshipmentStatusHandlerFactory = shipmentStatusHandlerFactory;
        }

        [HttpGet]
        public async Task<PagedResult<DtoShipment>> Get(int Pagenumber)
        {
            return await _shipmentService.GetShipmentsList(Pagenumber, 10, true, null);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Reviewer,Operation,Operation Manager")]

        public async Task<PagedResult<DtoShipment>> GetAll(int Pagenumber, int PageSize, bool Isuserdata, ShipmentStatusEnum? status = null)
        {
            return await _shipmentService.GetShipmentsList(Pagenumber, PageSize, Isuserdata, status);
        }

        [HttpGet("{id}")]
        public async Task<DtoShipment> Get(Guid id)
        {
            return await _shipmentService.GetShimpentAsync(id);
        }

        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin,Reviewer,Operation,Operation Manager")]
        public async Task<DtoShipment> GetByAdmin(Guid id)
        {
            return await _shipmentService.GetShipmentByAdminAsync(id);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Post([FromBody] DtoShipment value)
        {
            if (value == null)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(null, "Invalid shipment data."));
            }

            try
            {
                var result = await _shipmentCommand.CreateShipment(value);
                if (!result)
                {
                    return StatusCode(500, ApiResponse<string>.FailureResponse(null, "Failed to create shipment.", 500));
                }

                return Ok(ApiResponse<string>.SuccessResponse("", "Shipment created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse(null, "An error occurred while creating the shipment.", 500, new List<string> { ex.Message }));
            }
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] DtoShipment value)
        {
            if (value == null || value.Id == Guid.Empty)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(null, "Invalid shipment data."));
            }

            try
            {
                var result = await _shipmentCommand.EditAsync(value);
                if (!result)
                {
                    return StatusCode(500, ApiResponse<string>.FailureResponse(null, "Failed to update shipment.", 500));
                }

                return Ok(ApiResponse<string>.SuccessResponse("", "Shipment updated successfully."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(null, ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse(null, "An error occurred while updating the shipment.", 500, new List<string> { ex.Message }));
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid Shid)
        {
            var result = await _shipmentService.ChangeStatus(Shid, (int)ShipmentStatusEnum.Deleted);

            if (result)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Shipment Deleted Successfully",
                    Data = null,
                    StatusCode = 200
                });
            }

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Failed to delete shipment",
                Data = null,
                StatusCode = 400
            });
        }

        [HttpPost("ChangeStatus")]
        [Authorize(Roles = "Admin,Reviewer,Operation,Operation Manager")]
        public async Task<IActionResult> ChangeStatus([FromBody] DtoShipment Data)
        {
            if (Data.Id == Guid.Empty || !Enum.IsDefined(typeof(ShipmentStatusEnum), Data.CurrentState))
            {
                return BadRequest(ApiResponse<string>.FailureResponse(null, "Invalid shipment data."));
            }
                
            try
            {
                var result =  _IshipmentStatusHandlerFactory.GetStatusHandler((ShipmentStatusEnum)Data.CurrentState);
                await result.StatusHandler(Data);

                return Ok(ApiResponse<string>.SuccessResponse("", "Shipment updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse(null, "An error occurred while updating the shipment.", 500, new List<string> { ex.Message }));
            }

        }
    }
}
