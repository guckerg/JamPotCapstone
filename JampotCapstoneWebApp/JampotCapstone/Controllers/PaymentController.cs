using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using JampotCapstone.Models;

namespace JampotCapstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            // Retrieve credentials from configuration if needed
            var squareAppId = _configuration["Square:ApplicationId"];
            var squareLocationId = _configuration["Square:LocationId"];

            // Here, integrate with Square's .NET SDK or perform an HTTP call to Square's Payments API.
            // Below is a simplified placeholder for actual payment processing logic.
            bool paymentSucceeded = true;  // Replace with real processing logic

            if (paymentSucceeded)
            {
                return Ok(new { success = true, message = "Payment processed successfully." });
            }
            else
            {
                return BadRequest(new { success = false, error = "Payment processing failed." });
            }
        }
    }
}