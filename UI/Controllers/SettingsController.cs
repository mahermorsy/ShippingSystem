using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WebApi.UI_Services.APi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettings _settings;

        public SettingsController(ISettings settings)
        {
            _settings = settings;
        }
        // GET: api/<SettingsController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DtoSetting>>>> Get()
        {
            try
            {
                var shippingTypes = await _settings.GetAllAsync();
                if (!shippingTypes.Any())
                {
                    return Ok(ApiResponse<List<DtoSetting>>
                        .SuccessResponse(shippingTypes, "No Settings found"));
                }
                return Ok(ApiResponse<List<DtoSetting>>.SuccessResponse(shippingTypes, "Shipping types retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DtoSetting>>.FailureResponse(null, Dx.Message, 500, new List<string> { Dx.Message ,"An error occurred while retrieving Settings."}));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DtoSetting>>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving Settings." }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DtoSetting>>> Get(Guid id)
        {
            try
            {
                var shippingTypes = await _settings.GetByIdAsync(id);
                if (shippingTypes == null) 
                {
                    return NotFound(ApiResponse<DtoSetting>.FailureResponse(null,"Shipping type Not Found", 404));
                }
                return Ok(ApiResponse<DtoSetting>.SuccessResponse(shippingTypes, "Shipping type retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DtoSetting>.FailureResponse(null, Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving Settings." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DtoSetting>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving Settings." }));
            }
        }
        /*
        // POST api/<ShippingTypesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShippingTypesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShippingTypesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
