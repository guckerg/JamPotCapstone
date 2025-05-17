using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using File = JampotCapstone.Models.File;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoRepository _photoRepo;

        public MenuController(ApplicationDbContext ctx, IPhotoRepository p)
        {
            _context = ctx;
            _photoRepo = p;
        }
        public async Task<IActionResult> Index()
        {
            List<File> photos = await _photoRepo.GetPhotosByPageAsync("menu");
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
