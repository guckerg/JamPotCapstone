
using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JampotCapstone.Controllers
{
    public class CateringController : Controller
    {
        private ITextElementRepository _repo;
        private IPhotoRepository _photoRepo;

        public CateringController(ITextElementRepository r, IPhotoRepository p)
        {
            _repo = r;
            _photoRepo = p;
        }
        public async Task<IActionResult> Index()
        { 
            CateringViewModel model = new CateringViewModel();
            model.Textblocks = await _repo.GetTextElementsByPageAsync("catering");
            model.Photos = await _photoRepo.GetPhotosByPageAsync("catering");
            return View(model);
        }
    }
}
