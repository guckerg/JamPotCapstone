using Microsoft.AspNetCore.Mvc;

namespace JampotCapstone.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
