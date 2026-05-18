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
    public class CitiesController : ControllerBase
    {
        private readonly ICities _cities;
        private readonly IVwCityService _vwCityService;

        public CitiesController(ICities cities, IVwCityService vwCityService)
        {
            _cities = cities;
            _vwCityService = vwCityService;
        }
        // GET: api/<CitiesController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DtoCity>>>> Get()
        {
            try
            {
                var cities = await _cities.GetAllAsync();
                if (!cities.Any())
                {
                    return Ok(ApiResponse<List<DtoCity>>
                        .SuccessResponse(cities, "No cities found"));
                }
                return Ok(ApiResponse<List<DtoCity>>.SuccessResponse(cities, "cities retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DtoCity>>.FailureResponse(null,Dx.Message, 500, new List<string> { Dx.Message , "An error occurred while retrieving cities." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DtoCity>>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving cities." }));
            }

        }

        // GET api/<CitiesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DtoCity>>> Get(Guid id)
        {
            try
            {
                var cities = await _cities.GetByIdAsync(id);
                if (cities == null) 
                {
                    return NotFound(ApiResponse<DtoCity>.FailureResponse(null, "cities Not Found", 404));
                }
                return Ok(ApiResponse<DtoCity>.SuccessResponse(cities, "cities retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DtoCity>.FailureResponse(null, Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving cities." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DtoCity>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving cities." }));
            }
        }
        [HttpGet("Country/{id}")]
        public async Task<ActionResult<ApiResponse<List<VwCityDTO>>>> GetByCountryId(Guid id)
        {
            try

            {
                var cities = await _vwCityService.GetList(id);
                if (cities == null)
                {
                    return NotFound(ApiResponse<List<VwCityDTO>>.FailureResponse(null, "cities Not Found", 404));
                }
                return Ok(ApiResponse<List<VwCityDTO>>.SuccessResponse(cities, "cities retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<List<VwCityDTO>>.FailureResponse(null,   Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving cities." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<VwCityDTO>>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving cities." }));
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
