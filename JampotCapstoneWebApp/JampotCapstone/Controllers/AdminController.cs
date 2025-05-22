using System.Net.Mime;
using System.Reflection;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using File = JampotCapstone.Models.File;
using Microsoft.EntityFrameworkCore;
using File = JampotCapstone.Models.File;

namespace JampotCapstone.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ITextElementRepository _textRepo;
    private readonly IPhotoRepository _photoRepo;
    private readonly IPageRepository _pageRepo;
    private readonly IProductRepository _productRepo;

    public AdminController(ITextElementRepository t, IPhotoRepository ph, IPageRepository p, IProductRepository r)
    {
        _textRepo = t;
        _photoRepo = ph;
        _pageRepo = p;
        _productRepo = r;
    }
    
    public async Task<IActionResult> Index()
    {
        AdminViewModel model = new AdminViewModel
        {
            Textblocks = await _textRepo.GetAllTextElementsAsync(),
            Photos = await _photoRepo.GetAllPhotosAsync(),
            Products = await _productRepo.GetAllProductsAsync(),
            Pages = await _pageRepo.GetNonEmptyPagesAsync()
        };
        return View(model);
    }

    public async Task<IActionResult> Edit(int id = 0)
    {
        ViewBag.Pages = await _pageRepo.GetAllPagesAsync();
        TextElement? model = id == 0 ? new TextElement() // if an existing textblock was not sent to the controller, 
            : await _textRepo.GetTextElementByIdAsync(id);   // create a new one
        // ViewBag.Pages = _context.Pages.ToList();
        TextElement? model = id == 0 ? new TextElement() : await _context.TextElements
            .Include(t => t.PagePosition)
            .FirstOrDefaultAsync(t => t.TextElementId == id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TextElement model)
    {
        if (ModelState.IsValid)
        {
            int result = 0;
            if (model.TextElementId == 0) // id does not exist in the database, hence it is a new textblock
            if (model.TextElementId == 0) // new textblock, hence FAQ page
            {
                model.PagePosition.FAQs = _context.TextElements
                    .Count(t => t.PagePosition.FAQs != -1);
                _context.TextElements.Add(model);
            } else
                model.Page = await _pageRepo.GetPageByNameAsync("faq"); // creation of a new textblock requires that it be on the faq page
                result = await _textRepo.StoreTextElementAsync(model);
            } else // updating an existing record
            {
                result = await _textRepo.UpdateTextElementAsync(model);
            }
            if (result > 0)
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
            Position = id,
            Pages = await _pageRepo.GetAllPagesAsync(),
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
    public async Task<IActionResult> Add(File model)
    {
        if (ModelState.IsValid)
        {
            if (await _photoRepo.AddFileAsync(model) > 0)
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

    public async Task<IActionResult> Delete(int id)
    {
        TextElement? toDelete = await _textRepo.GetTextElementByIdAsync(id);
        if (toDelete != null)
        {
            if (await _textRepo.DeleteTextElementAsync(toDelete) > 0)
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

    public int ReplacePhoto(File oldFile, File newFile, string pageTitle)
    {
        int result = 0;
        // get a list of the properties in the class
        Type objType = newFile.PagePosition.GetType();
        PropertyInfo[] properties = objType.GetProperties();
        // iterate over the properties in the class
        foreach (PropertyInfo prop in properties)
        {
            string propertyName = prop.Name;
            // if the property name is the same as the provided page title
            if (pageTitle.ToLower().Contains(propertyName.ToLower())) 
            {
                // get the position of the old photo on the page
                var swapValue = prop.GetValue(oldFile.PagePosition);
                // put the new photo at that position
                prop.SetValue(newFile.PagePosition, swapValue);
                // remove the old photo from the page
                prop.SetValue(oldFile.PagePosition, -1);
                result = 1;
            }
        }

        return result;
    }
}