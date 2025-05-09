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
        public IActionResult Index()
        {
            TextElement? model = _context.TextElements.FirstOrDefault(t => t.Location.ToLower().Contains("about"));
            return View(model);
        }

        public async Task<IActionResult> Ask()
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
                    Where(t => t.Location.ToLower().Contains("faq")).ToListAsync(),
                Photo = photo
            };
                
            return View(model);
        }
        
    }
}
