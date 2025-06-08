using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.AspNetCore.Mvc;

namespace JampotCapstone.Controllers;

public class OrderController : Controller
{
    private readonly IProductRepository _prodRepo;

    public OrderController(IProductRepository prod)
    {
        _prodRepo = prod;
    }
    
    public async Task<IActionResult> Index()
    {
        List<Product> model = await _prodRepo.GetAllProductsAsync();
        return View(model);
    }

    public async Task<IActionResult> FilterByCategory(string type)
    {
        List<Product> model = await _prodRepo.GetProductsByCategoryAsync(type);
        return View("Index", model);
    }

    public async Task<IActionResult> FilterByTag(string type)
    {
        List<Product> model = await _prodRepo.GetProductsByTagAsync(type);
        return View("Index", model);
    }

    public async Task<IActionResult> Search(string key)
    {
        List<Product> model = await _prodRepo.GetProductsByNameAsync(key);
        return View("Index", model);
    }
}