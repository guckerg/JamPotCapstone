using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Data;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Specials()
        {
            ProductTag specials = await _context.ProductTags
                .Where(t => t.Tag.ToLower() == "special")
                    .Include(t => t.Products)
                        .ThenInclude(p => p.ProductPhoto)
                    .Include(t => t.Products)
                        .ThenInclude(p => p.ProductCategory).SingleOrDefaultAsync();
            return View(specials);
        }
    }
}
