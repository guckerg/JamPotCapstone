using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            CateringViewModel model = new CateringViewModel
            {
                Textblocks = await _repo.GetTextElementsByPage("catering"),
                Photos = await _photoRepo.GetPhotosByPageAsync("catering")
            };
            return View(model);
        }
    }
}
