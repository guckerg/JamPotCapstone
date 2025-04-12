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
}