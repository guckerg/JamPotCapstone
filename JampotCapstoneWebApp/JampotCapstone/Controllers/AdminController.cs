using System.Net.Mime;
using System.Reflection;
using JampotCapstone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using File = JampotCapstone.Models.File;

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

    public IActionResult Edit(int id = 0)
    {
        ViewBag.Pages = _context.Pages.ToList();
        TextElement? model = id == 0 ? new TextElement() : _context.TextElements.Find(id);
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(TextElement model)
    {
        if (ModelState.IsValid)
        {
            if (model.TextElementId == 0)
            {
                int id = model.PageId;
                model.Page = _context.Pages.Find(id);
                _context.TextElements.Add(model);
            } else
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

    public async Task<IActionResult> EditPhoto(int id, string pageTitle)
    {
        EditViewModel model = new EditViewModel
        {
            Photos = await _context.Files.Where(f => f.ContentType.ToLower().Contains("image")).ToListAsync(),
            CurrentPage = pageTitle,
            OldPhotoId = id
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditPhoto(EditViewModel model)
    {
        Models.File? newPhoto = await _context.Files.Include(f => f.PagePosition)
            .FirstOrDefaultAsync(f => f.FileID == model.NewPhotoId);
        Models.File? oldPhoto = await _context.Files.Include(f => f.PagePosition)
            .FirstOrDefaultAsync(f => f.FileID == model.OldPhotoId);
        if (ReplacePhoto(oldPhoto, newPhoto, model.CurrentPage) > 0)
        {
            _context.Files.Update(oldPhoto);
            _context.Files.Update(newPhoto);
            if (await _context.SaveChangesAsync() > 0)
            {
                TempData["Message"] = "Photo successfully changed.";
            }
            else
            {
                TempData["Message"] = "There was a problem saving the changes. Please try again.";
            }
        }
        else
        {
            TempData["Message"] = "Page not found. Please try again.";
        }
        /*Models.File? photo = await _context.Files.Where(f => f.FileName.ToLower().Contains(model.Key.ToLower()))
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
        */
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

    public IActionResult Delete(int id)
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

    public int ReplacePhoto(File oldFile, File newFile, string propName)
    {
        int result = 0;
        // get a list of the properties in the class
        Type objType = newFile.PagePosition.GetType();
        PropertyInfo[] properties = objType.GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            string propertyName = prop.Name;
            if (propertyName == propName)
            {
                var swapValue = prop.GetValue(oldFile.PagePosition);
                prop.SetValue(newFile.PagePosition, swapValue);
                prop.SetValue(oldFile.PagePosition, -1);
                result = 1;
            }
        }

        return result;
    }
}