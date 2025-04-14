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
        model.Map = await _context.Files.Where(
            m => m.FileName.Contains("map") && m.FileName.Contains("landing")).FirstOrDefaultAsync();
        model.Special = await _context.Files.Where(
            m => m.FileName.Contains("special") && m.FileName.Contains("landing")).FirstOrDefaultAsync();
        model.Photos = await _context.Files.Where(
            m => !m.FileName.Contains("map") && !m.FileName.Contains("special") 
                                             && m.FileName.Contains("landing")).ToListAsync();
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