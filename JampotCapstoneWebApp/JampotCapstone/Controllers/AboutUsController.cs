using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using SQLitePCL;
using File = JampotCapstone.Models.File;

namespace JampotCapstone.Controllers
{
    public class AboutUsController : Controller
    {
        private ITextElementRepository _repo;
        private IPhotoRepository _photoRepo;
        private IPageRepository _pageRepo;

        public AboutUsController(ITextElementRepository r, IPhotoRepository ph, IPageRepository p)
        {
            _repo = r;
            _photoRepo = ph;
            _pageRepo = p;
        }
        public async Task<IActionResult> Index()
        {
            // FOR TESTING
            AboutUsViewModel model = new AboutUsViewModel();
            var page = await _pageRepo.GetPageByNameAsync("about us");
            model.PageId = page.PageId;
            model.Textblock = await _repo.GetTextElementByPageAsync("about");
            model.Photos = await _photoRepo.GetPhotosByPageAsync("about");
            /*AboutUsViewModel model = new AboutUsViewModel
            {
                Textblock = await _repo.GetTextElementByPageAsync("about"),
                Photos = await _photoRepo.GetPhotosByPageAsync("about"),
                PageId = _pageRepo.GetPageByNameAsync("about").Result.PageId
            };*/
            return View(model);
        }

        public async Task<IActionResult> Ask()
        {
            File? photo = await _photoRepo.GetPhotoByPageAsync("faq");
            if (photo == null)
            {
                photo = await _photoRepo.GetFileByNameAsync("people");
            }
            ContentViewModel model = new ContentViewModel
            {
                Textblocks = await _repo.GetTextElementsByPageAsync("faq"),
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
