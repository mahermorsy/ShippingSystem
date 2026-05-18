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
    public class ShippingTypesController : ControllerBase
    {
        private readonly IShippingTypes _shippingTypes;

        public ShippingTypesController(IShippingTypes shippingTypes)
        {
            _shippingTypes = shippingTypes;
        }
        // GET: api/<ShippingTypesController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DtoShippingType>>>> Get()
        {
            try
            {
                var shippingTypes = await _shippingTypes.GetAllAsync();
                if (!shippingTypes.Any())
                {
                    return Ok(ApiResponse<List<DtoShippingType>>
                        .SuccessResponse(shippingTypes, "No shipping types found"));
                }
                return Ok(ApiResponse<List<DtoShippingType>>.SuccessResponse(shippingTypes, "Shipping types retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DtoShippingType>>.FailureResponse(null   ,Dx.Message, 500, new List<string> { Dx.Message ,"An error occurred while retrieving shipping types."}));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DtoShippingType>>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving shipping types." }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DtoShippingType>>> Get(Guid id)
        {
            try
            {
                var shippingTypes = await _shippingTypes.GetByIdAsync(id);
                if (shippingTypes == null) 
                {
                    return NotFound(ApiResponse<DtoShippingType>.FailureResponse(null,"Shipping type Not Found", 404));
                }
                return Ok(ApiResponse<DtoShippingType>.SuccessResponse(shippingTypes, "Shipping type retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DtoShippingType>.FailureResponse(null,Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving shipping types." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DtoShippingType>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving shipping types." }));
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
