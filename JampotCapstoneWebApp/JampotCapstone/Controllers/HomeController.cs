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

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx)
    {
        _context = ctx;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        HomeViewModel model = new HomeViewModel();
        model.Hours = await _context.TextElements.FirstOrDefaultAsync(t => t.Location.ToLower().Contains("home"));
        Page currentPage = await _context.Pages.Where(p => p.PageTitle.ToLower().Contains("home"))
            .Include(p => p.Files).FirstOrDefaultAsync();
        model.Photos = currentPage.Files;
        model.Special = model.Photos.Find(f => f.FileName.ToLower().Contains("special"));
        model.Photos.Remove(model.Special);
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}