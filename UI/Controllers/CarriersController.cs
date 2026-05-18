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
    public class CarriersController : ControllerBase
    {
        private readonly ICarriers _carriers;

        public CarriersController(ICarriers carriers)
        {
            _carriers = carriers;
        }
        // GET: api/<CarriersController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DtoCarrier>>>> Get()
        {
            try
            {
                var carriers = await _carriers.GetAllAsync();
                if (!carriers.Any())
                {
                    return Ok(ApiResponse<List<DtoCarrier>>
                        .SuccessResponse(carriers, "No carriers found"));
                }
                return Ok(ApiResponse<List<DtoCarrier>>.SuccessResponse(carriers, "Carriers retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DtoCarrier>>.FailureResponse(null    ,Dx.Message, 500, new List<string> { Dx.Message ,"An error occurred while retrieving shipping types."}));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DtoCarrier>>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving shipping types." }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DtoCarrier>>> Get(Guid id)
        {
            try
            {
                var carrier = await _carriers.GetByIdAsync(id);
                if (carrier == null) 
                {
                    return NotFound(ApiResponse<DtoCarrier>.FailureResponse(null,"Carrier Not Found", 404)); 
                }
                return Ok(ApiResponse<DtoCarrier>.SuccessResponse(carrier, "Carrier retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DtoCarrier>.FailureResponse(null,Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving shipping types." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DtoCarrier>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving shipping types." }));
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
