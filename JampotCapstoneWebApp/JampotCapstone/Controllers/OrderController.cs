using Microsoft.AspNetCore.Mvc;

namespace JampotCapstone.Controllers;

public class OrderController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}