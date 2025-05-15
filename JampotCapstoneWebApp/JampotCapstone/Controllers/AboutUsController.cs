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
        private ITextElementRepository _repo;

        public AboutUsController(ApplicationDbContext ctx, ITextElementRepository r)
        {
            _context = ctx;
            _repo = r;
        }
        public async Task<IActionResult> Index()
        {
            TextElement? model = await _repo.GetTextElementByPage("about");
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
                Textblocks = await _repo.GetTextElementsByPage("faq"),
                Photo = photo
            };
                
            return View(model);
        }
        
    }
}
