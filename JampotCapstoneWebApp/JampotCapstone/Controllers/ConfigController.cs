using Microsoft.AspNetCore.Mvc;

[Route("api/config")]
public class ConfigController : Controller
{
    private readonly IConfiguration _config;

    public ConfigController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("square")]
    public IActionResult GetSquareConfig()
    {
        var squareSettings = _config.GetSection("Square");
        var configData = new
        {
            ApplicationId = squareSettings["ApplicationId"],
            LocationId = squareSettings["LocationId"]
        };

        return Ok(configData);
    }
}