using System.Net;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using File = JampotCapstone.Models.File;

namespace JampotCapstone.Controllers
{
    public class AboutUsController : Controller
    {
        private ITextElementRepository _repo;
        private IPhotoRepository _photoRepo;

        public AboutUsController(ITextElementRepository r, IPhotoRepository p)
        {
            _repo = r;
            _photoRepo = p;
        }
        public async Task<IActionResult> Index()
        {
            TextElement? model = await _repo.GetTextElementByPageAsync("about");
            return View(model);
        }

        public async Task<IActionResult> Ask()
        {
            // get a list of the photos associated with this page
            File? photo = await _photoRepo.GetPhotoByPageAsync("faq");
            ContentViewModel model = new ContentViewModel
            {
                Textblocks = await _repo.GetTextElementsByPageAsync("faq")
            };
            if (photo == null) // if there are no photos currently associated with the page
            {
                model.Photo = await _photoRepo.GetPhotoByNameAsync("people"); // load a default image
            }
            else
            {
                model.Photo = photo;
            }
            return View(model);
        }
        
    }
}
