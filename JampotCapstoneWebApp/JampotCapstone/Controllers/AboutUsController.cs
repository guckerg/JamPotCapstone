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

        public async Task<IActionResult> Ask(string filename = "people.png")
        {
            ContentViewModel model = new ContentViewModel
            {
                Textblocks = await _context.TextElements.
                    Where(t => t.Location.ToLower().Contains("faq")).ToListAsync(),
                Photo = await _context.Files
                    .FirstOrDefaultAsync(f => f.FileName.ToLower().Contains(filename.ToLower()))
            };
                
            return View(model);
        }
        
    }
}
