using Microsoft.AspNetCore.Mvc;

namespace JampotCapstone.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
