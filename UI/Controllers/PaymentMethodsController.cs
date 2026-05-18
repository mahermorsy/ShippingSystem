using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WebApi.UI_Services.APi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethods _paymentMethods;

        public PaymentMethodsController(IPaymentMethods paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }
        // GET: api/<PaymentMethodsController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DtoPaymentMethod>>>> Get()
        {
            try
            {
                var paymentMethods = await _paymentMethods.GetAllAsync();
                if (!paymentMethods.Any())
                {
                    return Ok(ApiResponse<List<DtoPaymentMethod>>
                        .SuccessResponse(paymentMethods, "No paymentMethods found"));
                }
                return Ok(ApiResponse<List<DtoPaymentMethod>>.SuccessResponse(paymentMethods, "paymentMethods retrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500,ApiResponse<List<DtoPaymentMethod>>.FailureResponse(null,Dx.Message, 500, new List<string> { Dx.Message ,"An error occurred while retrieving paymentMethods."}));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<List<DtoPaymentMethod>>.FailureResponse(null, ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving paymentMethods." }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DtoPaymentMethod>>> Get(Guid id)
        {
            try
            {
                var paymentMethods = await _paymentMethods.GetByIdAsync(id);
                if (paymentMethods == null) 
                {
                    return NotFound(ApiResponse<DtoPaymentMethod>.FailureResponse(null, "paymentMethodsNot Found", 404));
                }
                return Ok(ApiResponse<DtoPaymentMethod>.SuccessResponse(paymentMethods, "paymentMethodsretrieved successfully", 200));
            }

            catch (DataAccessException Dx)
            {
                //  StatusCode() means : Server Error Not User Error.
                return StatusCode(500, ApiResponse<DtoPaymentMethod>.FailureResponse(null, Dx.Message, 500, new List<string> { Dx.Message, "An error occurred while retrieving paymentMethods." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DtoPaymentMethod>.FailureResponse(null,ex.Message, 500, new List<string> { ex.Message, "General Exception : error occurred while retrieving paymentMethods." }));
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
