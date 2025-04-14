using JampotCapstone.Data;
using JampotCapstone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductType> model = await _context.ProductTypes
                .Include(c => c.Products)
                    .ThenInclude(p => p.Tags)
                .Include(c => c.Products)
                    .ThenInclude(p => p.ProductPhoto).ToListAsync();
            return View(model);
        }
    }
}
