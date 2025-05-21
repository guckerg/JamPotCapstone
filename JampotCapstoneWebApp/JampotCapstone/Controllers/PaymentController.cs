using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using JampotCapstone.Models;
using Square;
using Square.Payments;

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
            var client = new SquareClient(
                token: _configuration["Square:AccessToken"],
                clientOptions: new ClientOptions
                {
                    BaseUrl = SquareEnvironment.Sandbox
                }
            );

            try {
                var response = await client.Payments.CreateAsync(
                    new CreatePaymentRequest
                    {
                        SourceId = request.Token,  // your token from tokenization
                        IdempotencyKey = Guid.NewGuid().ToString(),
                        AmountMoney = new Money { Amount = request.Amount, Currency = Currency.Usd },
                        Autocomplete = true,
                        LocationId = _configuration["Square:LocationId"]
                    }
                );

                if (response.Payment != null)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Payment processed successfully.",
                        paymentId = response.Payment.Id
                    });
                } else
                {
                    string errorMessage = string.Join(", ", response.Errors);
                    return BadRequest(new { success = false, error = errorMessage });
                }
            }
            catch (SquareApiException e)
            {
                Console.WriteLine(e.Body);
                Console.WriteLine(e.StatusCode);

                // Access the parsed error objects
                foreach (var error in e.Errors)
                {
                    Console.WriteLine($"Category: {error.Category}");
                    Console.WriteLine($"Code: {error.Code}");
                    Console.WriteLine($"Detail: {error.Detail}");
                    Console.WriteLine($"Field: {error.Field}");
                }
                return BadRequest(new { success = false, error = "my bad return message" });
            }
        }
    }
}