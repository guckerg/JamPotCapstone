using System.Diagnostics;
using JampotCapstone.Data;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ITextElementRepository _repo;
    private readonly IPhotoRepository _photoRepo;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx, ITextElementRepository r, IPhotoRepository p)
    {
        _context = ctx;
        _logger = logger;
        _repo = r;
        _photoRepo = p;
    }

    public async Task<IActionResult> Index()
    {
        HomeViewModel model = new HomeViewModel
        {
            Hours = await _repo.GetTextElementByPage("home"),   // get the text element associated with the page
            Photos = await _photoRepo.GetPhotosByPageAsync("home") // get the list of photos associated with the page
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}