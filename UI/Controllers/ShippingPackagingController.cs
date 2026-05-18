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
    public class ShippingPackagingController : ControllerBase
    {
        private readonly IShippingPackaging _shippingPackaging;

        public ShippingPackagingController(IShippingPackaging shippingPackaging)
        {
            _shippingPackaging = shippingPackaging;
        }
        // GET: api/<ShippingTypesController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DTOShippingPackaging>>>> Get()
        {
            try
            {
                var shippingPackaging = await _shippingPackaging.GetAllAsync();
                if (!shippingPackaging.Any())
                {
                    return Ok(ApiResponse<List<DTOShippingPackaging>>
                        .SuccessResponse(shippingPackaging, "No shipping packaging found"));
                }
                return Ok(ApiResponse<List<DTOShippingPackaging>>.SuccessResponse(shippingPackaging, "Shipping Packaging retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DTOShippingPackaging>>.FailureResponse(null,Dx.Message, 500, new List<string> { Dx.Message ,"An error occurred while retrieving Shipping Packaging."}));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DTOShippingPackaging>>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving Shipping Packaging." }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DTOShippingPackaging>>> Get(Guid id)
        {
            try
            {
                var shippingPackaging = await _shippingPackaging.GetByIdAsync(id);
                if (shippingPackaging == null) 
                {
                    return NotFound(ApiResponse<DTOShippingPackaging>.FailureResponse(null, "Shipping Packaging Not Found", 404));
                }
                return Ok(ApiResponse<DTOShippingPackaging>.SuccessResponse(shippingPackaging, "Shipping Packaging retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DTOShippingPackaging>.FailureResponse(null, Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving Shipping Packaging." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DTOShippingPackaging>.FailureResponse(null, ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving Shipping Packaging." }));
            }
        }
        /*
        // POST api/<ShippingPackagingController>
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
