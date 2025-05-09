using System.Net.Mime;
using JampotCapstone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext ctx)
    {
        _context = ctx;
    }

    public async Task<IActionResult> Index()
    {
        AdminViewModel model = new AdminViewModel
        {
            Textblocks = await _context.TextElements.OrderBy(t => t.Location).ToListAsync(),
            Photos = await _context.Files.ToListAsync(),
            Products = await _context.Products.ToListAsync()
        };
        return View(model);
    }

    public IActionResult TextEdit(int id = 0)
    {
        TextElement? model = id == 0 ? new TextElement() : _context.TextElements.Find(id);
        return View(model);
    }

    [HttpPost]
    public IActionResult TextEdit(TextElement model)
    {
        if (ModelState.IsValid)
        {
            if (model.TextElementId == 0)
            {
                _context.TextElements.Add(model);
            }
            else
            {
                _context.TextElements.Update(model);
            }
            if (_context.SaveChanges() > 0)
            {
                TempData["Message"] = "Element successfully updated.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Changes could not be saved. Please try again.";
            }
        }
        else
        {
            TempData["Message"] = "There were data-entry errors. Please check the form.";
            TempData["context"] = "danger";
        }
        return View(model);
    }

    public IActionResult DeleteText(int id)
    {
        TextElement? toDelete = _context.TextElements.Find(id);
        if (toDelete != null)
        {
            _context.TextElements.Remove(toDelete);
            if (_context.SaveChanges() > 0)
            {
                TempData["Message"] = "Text block successfully deleted.";
                TempData["context"] = "success";
            }
        }
        else
        {
            TempData["Message"] = "That text block was not found. Please try again.";
            TempData["context"] = "danger";
        }

        return View("Index");
    }

    public IActionResult ProductEdit(int id = 0)
    {
        var categories = _context.Products.Select(c => new { c.ProductId, c.ProductCategory }).ToList();
        Product? model = id == 0 ? new Product() : _context.Products.Find(id);
        return View(model);
    }

    [HttpPost]
    public IActionResult ProductEdit(Product model)
    {
        if (ModelState.IsValid)
        {
            if (model.ProductId == 0)
            {
                _context.Products.Add(model);
            }
            else
            {
                _context.Products.Update(model);
            }
            if (_context.SaveChanges() > 0)
            {
                TempData["Message"] = "Element successfully updated.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Changes could not be saved. Please try again.";
            }
        }
        else
        {
            TempData["Message"] = "There were data-entry errors. Please check the form.";
            TempData["context"] = "danger";
        }
        return View(model);
    }

    public IActionResult DeleteProduct(int id)
    {
        Product? toDelete = _context.Products.Find(id);
        if (toDelete != null)
        {
            _context.Products.Remove(toDelete);
            if (_context.SaveChanges() > 0)
            {
                TempData["Message"] = "Product successfully deleted.";
                TempData["context"] = "success";
            }
        }
        else
        {
            TempData["Message"] = "The product was not found. Please try again.";
            TempData["context"] = "danger";
        }

        return View("Index");
    }
}