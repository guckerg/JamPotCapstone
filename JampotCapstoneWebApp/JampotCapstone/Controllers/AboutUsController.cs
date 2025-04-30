using JampotCapstone.Data;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
namespace JampotCapstone.Controllers
{
    public class AboutUsController : Controller
    {
        private ApplicationDbContext _context;

        public AboutUsController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public IActionResult Index()
        {
            TextElement? model = _context.TextElements.SingleOrDefault(t => t.Location.ToLower().Contains("aboutus"));
            return View(model);
        }

        public IActionResult Ask()
        {
            return View();
        }
        
    }
}
