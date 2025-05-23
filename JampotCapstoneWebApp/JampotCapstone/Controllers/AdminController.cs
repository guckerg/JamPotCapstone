using System.Net.Mime;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using File = JampotCapstone.Models.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.AspNetCore.Hosting;

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

    public async Task<IActionResult> TextEdit(int id = 0)
    {
        ViewBag.Pages = await _pageRepo.GetAllPagesAsync();
        TextElement? model = id == 0 ? new TextElement() // if an existing textblock was not sent to the controller, 
            : await _textRepo.GetTextElementByIdAsync(id);   // create a new one
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> TextEdit(TextElement model)
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

    public async Task<IActionResult> DeleteText(int id)
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

    public IActionResult AddPhoto()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddPhoto(File model)
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

    public async Task<IActionResult> ProductEdit(int id = 0)
    {
        // Create a list of tag and type objects for the drop down in the view
        var tags = await _productRepo.GetAllProductTagsAsync();
        var types = await _productRepo.GetAllProductTypesAsync();
        // Create a list of all the products in the database
        var products = await _productRepo.GetAllProductsAsync();

        Product? model = id == 0
            ? new Product()
            : await _productRepo.GetProductByIdAsync(id);

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
            //PhotoUpload = model.ProductPhoto.FilePat, handle image casting TODO
            SelectedTypeId = model.ProductCategory?.FirstOrDefault()?.TypeId ?? 0, //Get the currently selected type
            // Populate drop down
            Types = new SelectList(types, "TypeId", "Type", model.ProductCategory?.FirstOrDefault()?.TypeId),
            SelectedTagIds = model.Tags.Select(t => t.TagID).ToList(), //Currently selected tags
            Tags = tags.Select(tag => new SelectListItem
            {
                Value = tag.TagID.ToString(),
                Text = tag.Tag,
                Selected = model.Tags.Any(t => t.TagID == tag.TagID)
            }).ToList(),
        };

        return View(viewModel);
    }

    private async Task<File?> SaveProductImageAsync(IFormFile? photoUpload)
    {
        if (photoUpload == null || photoUpload.Length == 0)
            return null;

        string productPhotosFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/productPhotos");
        Directory.CreateDirectory(productPhotosFolder);

        string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(photoUpload.FileName);
        string filePath = Path.Combine(productPhotosFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await photoUpload.CopyToAsync(stream);
        }

        return new File { FileName = "/productPhotos/" + uniqueFileName };
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductEdit(int id, ProductEditViewModel viewModel)
    {
        if (id != viewModel.ProductId)
        {
            TempData["Message"] = "The product was not found.";
            TempData["context"] = "danger";
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            // Repopulate dropdowns on error
            var tags = await _productRepo.GetAllProductTagsAsync();
            var types = await _productRepo.GetAllProductTypesAsync();
            viewModel.Types = new SelectList(types, "TypeId", "Type", viewModel.SelectedTypeId);
            viewModel.Tags = tags.Select(tag => new SelectListItem
            {
                Value = tag.TagID.ToString(),
                Text = tag.Tag,
                Selected = viewModel.SelectedTagIds.Contains(tag.TagID)
            }).ToList();

            TempData["Message"] = "There were data-entry errors. Please check the form.";
            TempData["context"] = "danger";
            return View(viewModel);
        }

        // Handle file upload
        var uploadedImage = await SaveProductImageAsync(viewModel.PhotoUpload);

        var productToSave = new Product
        {
            ProductId = viewModel.ProductId,
            ProductName = viewModel.ProductName,
            ProductPrice = viewModel.ProductPrice,
            ProductIngredients = viewModel.ProductIngredients,
            ProductPhoto = uploadedImage ?? new File(), // default if no photo
            ProductCategory = new List<ProductType>
            {
                await _productRepo.GetProductTypeByIdAsync(viewModel.SelectedTypeId)
            },
            Tags = await _productRepo.GetTagsByIdsAsync(viewModel.SelectedTagIds),
        };

        if (viewModel.ProductId == 0)
        {
            await _productRepo.AddProductAsync(productToSave);
            TempData["Message"] = "Element successfully added.";
        }
        else
        {
            await _productRepo.UpdateProductAsync(productToSave);
            TempData["Message"] = "Element successfully updated.";
        }

        TempData["context"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        // find the product to delete by the id passed through
        Product? toDelete = await _productRepo.GetProductByIdAsync(id);
        if (toDelete != null)
        {
            await _productRepo.DeleteProductAsync(toDelete);
            TempData["Message"] = "Product successfully deleted.";
            TempData["context"] = "success";
        }
        else
        {
            TempData["Message"] = "The product was not found. Please try again.";
            TempData["context"] = "danger";
        }

        return RedirectToAction("Index");
    }
}