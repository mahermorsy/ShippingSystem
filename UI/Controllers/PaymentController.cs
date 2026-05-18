using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Services.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayPalGateway _payPalGateway;

        public PaymentController(PaymentFactory paymentFactory)
        {
            _payPalGateway = paymentFactory.CreatePayPalGateway("US"); // Example: Create PayPal gateway for Egypt
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment(CreatePaymentRequist PRequist)
        {
            var result = await _payPalGateway.CreateOrder(PRequist);
            if (result.Item2)
            {
                return Ok(new { Message = "Payment processed successfully.", OrderID = result.Item1 });
            }
            else
            {
                return BadRequest(new { Message = "Payment processing failed.", Details = ReadGatewayResponse(result.Item1) });
            }
        }

        [HttpPost("Capture")]
        public async Task<IActionResult> CapturePayment(DtoPaypalPayment Order)
        {
            var result = await _payPalGateway.CaptureOrde(Order.OrderId);
            if (result.Item2)
            {
                return Ok(ReadGatewayResponse(result.Item1));
            }
            else
            {
                return BadRequest(new { Message = "Payment capture failed.", Details = ReadGatewayResponse(result.Item1) });
            }
        }

        private static object ReadGatewayResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return new { Message = "Empty PayPal response." };
            }

            try
            {
                return JsonSerializer.Deserialize<JsonElement>(response);
            }
            catch (JsonException)
            {
                return new { Message = response };
            }
        }
    }
}
