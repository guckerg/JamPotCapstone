using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    // GET
    public IActionResult Index()
    {
        OrderViewModel model = new OrderViewModel();
        model.Products = _context.Products.Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .Include(p => p.ProductPhoto)
            .OrderBy(p => p.ProductCategory.First()).ToList();
        model.ProductTypes = _context.ProductTypes.ToList();
        return View(model);
    }

    public async Task<IActionResult> FilterByCategory(string type)
    {
        OrderViewModel model = new OrderViewModel();
        model.ProductTypes = await _context.ProductTypes.ToListAsync();
        model.ProductTypes = model.ProductTypes.Where(c => c.Type.ToLower() == type.ToLower());
        model.Products = await _context.Products.Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .Include(p => p.ProductPhoto)
            .OrderBy(p => p.ProductCategory.First()).ToListAsync();
        return View("Index", model);
    }

    public async Task<IActionResult> FilterByTag(string type)
    {
        OrderViewModel model = new OrderViewModel();
        model.ProductTypes = await _context.ProductTypes.ToListAsync();
        model.Products = await _context.Products.Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .Include(p => p.ProductPhoto)
            .OrderBy(p => p.ProductCategory.First()).ToListAsync();
        model.Products = model.Products.Where(p => p.Tags.Where(t => t.Tag.ToLower() == type.ToLower()).ToList().Any());
        return View("Index", model);
    }

    public async Task<IActionResult> Search(string key)
    {
        OrderViewModel model = new OrderViewModel();
        model.ProductTypes = await _context.ProductTypes.ToListAsync();
        model.Products = await _context.Products.Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .Include(p => p.ProductPhoto)
            .OrderBy(p => p.ProductCategory.First()).ToListAsync();
        model.Products = model.Products.Where(p => p.ProductName.ToLower().Contains(key.ToLower()));
        return View("Index", model);
    }
}