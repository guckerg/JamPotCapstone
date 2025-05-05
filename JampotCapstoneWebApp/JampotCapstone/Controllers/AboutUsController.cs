using JampotCapstone.Data;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

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
            TextElement? model = _context.TextElements.FirstOrDefault(t => t.Location.ToLower().Contains("aboutus"));
            return View(model);
        }

        public async Task<IActionResult> Ask()
        {
            List<TextElement> model =
                await _context.TextElements.
                    Where(t => t.Location.ToLower().Contains("faq")).ToListAsync();
            return View(model);
        }
        
    }
}
