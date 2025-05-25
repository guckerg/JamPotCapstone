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
            AboutUsViewModel model = new AboutUsViewModel();
            model.PageId = _pageRepo.GetPageByNameAsync("about us").Result.PageId;
            model.Textblock = await _repo.GetTextElementByPageAsync("about");
            model.Photos = await _photoRepo.GetPhotosByPageAsync("about");
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
        
    }
}
