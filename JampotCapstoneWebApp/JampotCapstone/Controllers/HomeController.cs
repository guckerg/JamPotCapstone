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

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx, ITextElementRepository r)
    {
        _context = ctx;
        _logger = logger;
        _repo = r;
    }

    public async Task<IActionResult> Index()
    {
        // get the page for this method so as to load the photos associated with it
        Page currentPage = await _context.Pages
            .Where(p => p.PageTitle.ToLower().Contains("home"))
            .Include(p => p.Files)  // associated photos
            .FirstOrDefaultAsync(); // there should be only one page that meets the criteria
        HomeViewModel model = new HomeViewModel
        {
            Hours = await _repo.GetTextElementByPage("home"),   // get the text element associated with the page
            Photos = currentPage == null ? [] : currentPage.Files
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}