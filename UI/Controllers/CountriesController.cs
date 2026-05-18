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
    public class CountriesController : ControllerBase
    {
        private readonly ICountries _Icountries;

        public CountriesController(ICountries Icountries)
        {
            _Icountries = Icountries;
        }
        // GET: api/<CountriesController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DtoCounty>>>> Get()
        {
            try
            {
                var Icountries = await _Icountries.GetAllAsync();
                if (!Icountries.Any())
                {
                    return Ok(ApiResponse<List<DtoCounty>>
                        .SuccessResponse(Icountries, "No Countries found"));
                }
                return Ok(ApiResponse<List<DtoCounty>>.
                    SuccessResponse(Icountries, "Countries retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DtoCounty>>.
                    FailureResponse(null,Dx.Message, 500, new List<string> { Dx.Message ,"An error occurred while retrieving Countries."}));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DtoCounty>>.
                    FailureResponse(null, ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving Countries." }));
            }

        }

        // GET api/<CountriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DtoCounty>>> Get(Guid id)
        {
            try { 
                var Icountries = await _Icountries.GetByIdAsync(id);
                if (Icountries == null) 
                {
                    return NotFound(ApiResponse<DtoCounty>.
                        FailureResponse(null, "Countries Not Found", 404));
                }
                return Ok(ApiResponse<DtoCounty>.
                    SuccessResponse(Icountries, "Countries retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DtoCounty>.
                    FailureResponse(null, Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving Countries." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DtoCounty>.
                    FailureResponse(null, ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving Countries." }));
            }
        }
      
    }
}
