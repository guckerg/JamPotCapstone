using System.Net.Mime;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using File = JampotCapstone.Models.File;
using Microsoft.EntityFrameworkCore;

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
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TextElement model)
    {
        if (ModelState.IsValid)
        {
            int result = 0;
            if (model.TextElementId == 0) // id does not exist in the database, hence it is a new textblock
            {
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

    public async Task<IActionResult> EditPhoto(int id)
    {
        EditViewModel model = new EditViewModel
        {
            Position = id,
            Pages = await _pageRepo.GetAllPagesAsync(),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditPhoto(EditViewModel model)
    {
        int result = 0;
        File? photo = await _photoRepo.GetFileByNameAsync(model.Key);
        Page? currentPage = await _pageRepo.GetPageByIdAsync(model.Page);
        if (currentPage != null)
        {
            if (currentPage.Files.Count > 0)
            {
                File oldPhoto = await _photoRepo.GetFileByIdAsync(model.Position);
                int index = currentPage.Files.IndexOf(oldPhoto);
                currentPage.Files[index] = photo;
            }
            else
            {
                currentPage.Files.Add(photo);
            }

            result = await _pageRepo.UpdatePageAsync(currentPage);
        }

        if (result > 0)
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
}