using Microsoft.AspNetCore.Mvc;

namespace JampotCapstone.Controllers
{
    public class CareersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
