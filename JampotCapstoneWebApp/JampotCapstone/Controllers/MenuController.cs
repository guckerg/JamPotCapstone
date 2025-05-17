using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using File = JampotCapstone.Models.File;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class MenuController : Controller
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly IProductRepository _productRepo;

        public MenuController(IPhotoRepository p, IProductRepository prod)
        {
            _photoRepo = p;
            _productRepo = prod;
        }
        public async Task<IActionResult> Index()
        {
            List<File> photos = await _photoRepo.GetPhotosByPageAsync("menu");
            return View(photos);
        }

        public async Task<IActionResult> Specials()
        {
            SpecialViewModel model = new SpecialViewModel
            {
                Specials = await _productRepo.GetProductsByTagAsync("special"),
                Promotions = await _photoRepo.GetFilesByNameAsync("special")
            };
            return View(model);
        }
    }
}
