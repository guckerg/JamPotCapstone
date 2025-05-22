using JampotCapstone.Data;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
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
        public async Task<IActionResult> Index()
        {
            AboutUsViewModel model = new AboutUsViewModel
            {
                Textblock = _context.TextElements
                    .FirstOrDefault(t => t.Page.PageTitle.ToLower().Contains("about")),
                Photos = await _context.Files.Include(f => f.PagePosition)
                    .Where(f => f.PagePosition.About != -1).OrderBy(f => f.PagePosition.About)
                    .ToListAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Ask()
        {
            Models.File? photo = await _context.Files.Include(f => f.PagePosition)
                .SingleOrDefaultAsync(f => f.PagePosition.FAQs != -1);
            if (photo == null)
            {
                photo = await _context.Files.FirstOrDefaultAsync(f => f.FileName.ToLower().Contains("people"));
            }
            ContentViewModel model = new ContentViewModel
            {
                Textblocks = await _context.TextElements.
                    Where(t => t.Page.PageTitle.ToLower().Contains("faq")).ToListAsync(),
                Photo = photo
            };
            return View(model);
        }

        /*public async Task<IActionResult> Ask()
        {
            Page? currentPage = await _context.Pages.Where(p => p.PageTitle.ToLower() == "faq")
                .Include(p => p.Files)
                .FirstOrDefaultAsync();
            Models.File photo;
            if (currentPage.Files.Count == 0)
            {
                photo = await _context.Files.FirstOrDefaultAsync(f => f.FileName.ToLower().Contains("people")); // default image
            }
            else
            {
                photo = currentPage.Files.FirstOrDefault();                
            }
            ContentViewModel model = new ContentViewModel
            {
                Textblocks = await _context.TextElements.
                    Where(t => t.Page.PageTitle.ToLower().Contains("faq")).ToListAsync(),
                Photo = photo
            };
                
            return View(model);
        }*/
        
    }
}
