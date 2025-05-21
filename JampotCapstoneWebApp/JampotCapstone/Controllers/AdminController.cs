using System.Net.Mime;
using JampotCapstone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

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
            Textblocks = await _context.TextElements.OrderBy(t => t.Page)
                .Include(t => t.Page).ToListAsync(),
            Photos = await _context.Files.ToListAsync(),
            Products = await _context.Products.ToListAsync(),
            Pages = await _context.Pages.Where(p => p.Files.Count > 0).Include(p => p.Files).ToListAsync(),
        };
        return View(model);
    }

    public IActionResult TextEdit(int id = 0)
    {
        ViewBag.Pages = _context.Pages.ToList();
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
                int id = model.PageId;
                model.Page = _context.Pages.Find(id);
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

    public IActionResult EditPhoto(int id)
    {
        EditViewModel model = new EditViewModel
        {
            Position = id,
            Pages = _context.Pages.ToList(),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditPhoto(EditViewModel model)
    {
        Models.File? photo = await _context.Files.Where(f => f.FileName.ToLower().Contains(model.Key.ToLower()))
            .Include(f => f.Pages)
            .FirstOrDefaultAsync();
        Page? currentPage = await _context.Pages.Include(p => p.Files)
            .FirstOrDefaultAsync(p => p.PageId == model.Page);
        if (currentPage.Files.Count > 0)
        {
            Models.File oldPhoto = currentPage.Files.Find(f => f.FileID == model.Position);
            int index = currentPage.Files.IndexOf(oldPhoto);
            currentPage.Files[index] = photo;
        }
        else
        {
            currentPage.Files.Add(photo);
        }
        _context.Pages.Update(currentPage);
        if (await _context.SaveChangesAsync() > 0)
        {
            TempData["Message"] = "Photo successfully changed.";
        }
        else
        {
            TempData["Message"] = "There was a problem saving the changes. Please try again.";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Models.File model)
    {
        if (ModelState.IsValid)
        {
            _context.Files.Add(model);
            if (await _context.SaveChangesAsync() > 0)
            {
                TempData["Message"] = "File successfully added.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "There was a problem saving the file. Please try again.");
            }
        }
        else
        {
            ModelState.AddModelError("", "There were data-entry errors. Please check the form.");
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

        return RedirectToAction("Index");
    }

    public IActionResult ProductEdit(int id = 0)
    {
        // Create a list of tag and type objects 
        var tags = _context.ProductTags.ToList();
        var types = _context.ProductTypes.ToList();

        Product? model = id == 0
            ? new Product()
            : _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.ProductId == id);

        if (model == null)
        {
            TempData["Message"] = "Product not found.";
            return RedirectToAction("Index");
        }

        // Populate the view model from the product
        var viewModel = new ProductEditViewModel
        {
            ProductId = model.ProductId,
            ProductName = model.ProductName,
            ProductPrice = model.ProductPrice,
            ProductIngredients = model.ProductIngredients,
            Tags = new SelectList(tags, "TagID", "Tag", model.Tags?.FirstOrDefault()?.TagID),
            Types = new SelectList(types, "TypeId", "Type", model.ProductCategory?.FirstOrDefault()?.TypeId)
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ProductEdit(ProductEditViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (viewModel.ProductId == 0)
            {
                var model = viewModel.Product;
                _context.Products.Add(model);
            }
            else
            {
                var model = viewModel.Product;
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
            viewModel.Tags = new SelectList(_context.ProductTags, "TagID", "Tag");
            viewModel.Types = new SelectList(_context.ProductTypes, "TypeId", "Type");
            TempData["Message"] = "There were data-entry errors. Please check the form.";
            TempData["context"] = "danger";
            return View("Index", viewModel);
        }
        return View(viewModel);
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