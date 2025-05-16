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
        private IPhotoRepository _photoRepo;

        public AboutUsController(ApplicationDbContext ctx, ITextElementRepository r, IPhotoRepository p)
        {
            _context = ctx;
            _repo = r;
            _photoRepo = p;
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
            if (currentPage != null && currentPage.Files.Count == 0) // if there are no photos currently associated with the page
            {
                photo = await _photoRepo.GetPhotoByNameAsync("people"); // load a default image
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
