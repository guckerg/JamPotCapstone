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
    public async Task<IActionResult> Index()
    {
        List<ProductType> model = await _context.ProductTypes
            .Include(c => c.Products)
                .ThenInclude(p => p.Tags)
            .Include(c => c.Products)
                .ThenInclude(p => p.ProductPhoto).ToListAsync();
        return View(model);
    }

    public async Task<IActionResult> FilterByCategory(string type)
    {
        List<ProductType> model = await _context.ProductTypes
            .Where(c => c.Type.ToLower() == type.ToLower())
            .Include(c => c.Products)
                .ThenInclude(p => p.ProductPhoto)
            .Include(c => c.Products)
                .ThenInclude(p => p.Tags)
            .ToListAsync();
        return View("Index", model);
    }

    public async Task<IActionResult> FilterByTag(string type)
    {
        List<ProductType> model = await _context.ProductTypes
            .Include(c => c.Products
                .Where(p => p.Tags
                    .Any(t => t.Tag.ToLower() == type.ToLower())))
            .ThenInclude(p => p.ProductPhoto)
            .ToListAsync();
        return View("Index", model);
    }

    public async Task<IActionResult> Search(string key)
    {
        List<ProductType> model = await _context.ProductTypes
            .Include(c => c.Products
                .Where(p => p.ProductName.ToLower().Contains(key)))
            .ThenInclude(p => p.ProductPhoto)
            .Include(c => c.Products)// have to reorient to the top level to get another item from the same level
                .ThenInclude(p => p.Tags)
            .ToListAsync();
        return View("Index", model);
    }
}