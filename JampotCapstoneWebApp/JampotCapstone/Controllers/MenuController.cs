using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
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
            Page currentPage = _context.Pages.Where(p => p.PageTitle.ToLower().Contains("menu"))
                .Include(p => p.Files)
                .FirstOrDefault();
            List<Models.File> photos = currentPage.Files;
            return View(photos);
        }

        public async Task<IActionResult> Specials()
        {
            SpecialViewModel model = new SpecialViewModel
            {
                Specials = await _context.ProductTags
                    .Where(t => t.Tag.ToLower() == "special")
                    .Include(t => t.Products)
                    .ThenInclude(p => p.ProductPhoto)
                    .Include(t => t.Products)
                    .ThenInclude(p => p.ProductCategory).SingleOrDefaultAsync(),
                Promotions = await _context.Files.Where(f => f.FileName.Contains("special")).ToListAsync()
            };
            return View(model);
        }
    }
}
