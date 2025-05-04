using JampotCapstone.Data;
using JampotCapstone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CateringController : Controller
    {
        private ApplicationDbContext _context;

        public CateringController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public async Task<IActionResult> Index()
        {
            List<TextElement> model = await _context.TextElements
                .Where(t => t.Location.ToLower().Contains("catering")).ToListAsync();   
            return View(model);
        }
    }
}
