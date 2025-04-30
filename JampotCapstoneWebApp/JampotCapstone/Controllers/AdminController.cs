using JampotCapstone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;

namespace JampotCapstone.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Edit(int id)
    {
        TextElement model = _context.TextElements.Find(id);
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(TextElement model)
    {
        if (ModelState.IsValid)
        {
            _context.TextElements.Update(model);
            if (_context.SaveChanges() > 0)
            {
                return 
            }
        }
    }
}