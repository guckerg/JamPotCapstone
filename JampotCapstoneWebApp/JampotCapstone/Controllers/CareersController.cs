using JampotCapstone.Data;
using JampotCapstone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CareersController : Controller
    {
        private ApplicationDbContext _context;

        public CareersController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Text =
                await _context.TextElements
                    .SingleOrDefaultAsync(t => t.Location.ToLower().Contains("careers"));
            return View();
        }
    }
}
