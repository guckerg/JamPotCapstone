using System.Diagnostics;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;

namespace JampotCapstone.Controllers;

public class HomeController : Controller
{
    private readonly ITextElementRepository _repo;
    private readonly IPhotoRepository _photoRepo;

    public HomeController(ITextElementRepository r, IPhotoRepository p)
    {
        _repo = r;
        _photoRepo = p;
    }

    public async Task<IActionResult> Index()
    {
        HomeViewModel model = new HomeViewModel
        {
            Hours = await _repo.GetTextElementByPageAsync("home"),
            Photos = await _photoRepo.GetPhotosByPageAsync("home")
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}