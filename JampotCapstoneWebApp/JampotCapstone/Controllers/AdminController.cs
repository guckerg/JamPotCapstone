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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JampotCapstone.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ITextElementRepository _textRepo;
    private readonly IPhotoRepository _photoRepo;
    private readonly IPageRepository _pageRepo;
    private readonly IProductRepository _productRepo;
    private readonly IPagePositionRepository _pagePositionRepo;
    private readonly IApplicationRepository _applicationRepo;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ApplicationDbContext _context;

    public AdminController(ITextElementRepository t, IPhotoRepository ph, IPageRepository p, IProductRepository r, 
        IApplicationRepository ar, IPagePositionRepository pp, IWebHostEnvironment he, ApplicationDbContext c)
    {
        _textRepo = t;
        _photoRepo = ph;
        _pageRepo = p;
        _productRepo = r;
        _pagePositionRepo = pp;
        _applicationRepo = ar;
        _hostingEnvironment = he;
        _context = c;
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
                if(_textRepo.GetType() == typeof(TextElementRepository)) // for unit testsx
                {
                    TempData["Message"] = "Element successfully updated.";
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Changes could not be saved. Please try again.");
            }
        }
        else
        {
            ModelState.AddModelError("", "There were data-entry errors. Please check the form.");
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
    public async Task<IActionResult> EditPhoto(int id, string pageTitle)
    {
        EditViewModel model = new EditViewModel
        {
            Photos = await _photoRepo.GetPhotosNotInPageAsync(pageTitle),
            CurrentPage = pageTitle,
            OldPhotoId = id
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditPhoto(EditViewModel model)
    {
        if (ModelState.IsValid)
        {
            int pageId = _pageRepo.GetPageByNameAsync(model.CurrentPage).Result.PageId;
            PagePosition? oldInstance = await _pagePositionRepo.GetPagePosition(pageId, model.OldPhotoId);
            if (oldInstance != null)
            {
                oldInstance.FileId = model.NewPhotoId;

                if (await _pagePositionRepo.UpdatePagePosition(oldInstance) > 0)
                {
                    if (_photoRepo.GetType() == typeof(PhotoRepository)) // otherwise unit tests fail
                    {
                        TempData["Message"] = "Photo successfully changed.";

                    }

                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "There was a problem saving the changes. Please try again.");
            }
        } else
        {
            ModelState.AddModelError("", "Something went wrong sending the form. Please try again.");
        }
        return View(model);
    }

    public IActionResult AddPhoto()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddPhoto(IFormFile newFile)
    {
        File newPhoto;
        // check that a file was sent to the controller
        if (newFile != null)
        {
            newPhoto = await SaveImageAsync(newFile, "pics");
            if (newPhoto == null)
            {
                if (_photoRepo.GetType() == typeof(PhotoRepository))
                {
                    TempData["Message"] = "Invalid image file. Only JPG, JPEG, PNG, " +
                                          "or WebP images up to 10MB are allowed.";
                    
                }
                return View(newFile);
            }
            newPhoto.ContentType = newFile.ContentType.ToLowerInvariant();
            if (await _photoRepo.AddFileAsync(newPhoto) > 0)
            {
                if (_photoRepo.GetType() == typeof(PhotoRepository))
                {
                    TempData["Message"] = "Photo successfully added.";
                }
            }
            else
            {
                TempData["Message"] = "There was a problem saving the file. Please try again.";
            }
        } else {
            TempData["Message"] = "File could not be uploaded. Please try again.";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeletePhoto(int id)
    {
        File photoToDelete = await _photoRepo.GetFileByIdAsync(id);
        if (photoToDelete == null)
        {
            TempData["Message"] = "File not found. Please try again.";
            TempData["context"] = "danger";
        }
        else
        {
            if (await _photoRepo.DeleteFileAsync(photoToDelete) > 0)
            {
                // proceed with local deletion
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + photoToDelete.FileName);
                FileInfo fileToDelete = new FileInfo(path); // source: https://www.c-sharpcorner.com/UploadFile/9f0ae2/delete-files-from-folder-in-Asp-Net/
                if (fileToDelete.Exists)
                {
                    fileToDelete.Delete();
                    TempData["Message"] = "Photo successfully deleted.";
                    TempData["context"] = "success";
                }
                else
                {
                    TempData["Message"] = "A local instance of the file could not be found.";
                    TempData["context"] = "danger";
                }
            }
            else
            {
                TempData["Message"] = "There was a problem deleting the file entry.";
                TempData["context"] = "danger";
            }
        }
        return RedirectToAction("Index");
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
            if (_productRepo.GetType() == typeof(ProductRepository)) // otherwise unit tests fail
            {
                TempData["Message"] = "Product not found.";
            }
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

    public async Task<File?> SaveImageAsync(IFormFile? photoUpload, string photoFolder)
    {
        if (photoUpload == null || photoUpload.Length == 0)
            return null; // No file uploaded or empty file, no validation needed

        // Define allowed image extensions and MIME types
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp" }; // Use MIME types for stronger validation
        // MIME stands for Multipurpose Internet Mail Extensions

        var fileExtension = Path.GetExtension(photoUpload.FileName).ToLowerInvariant();
        var mimeType = photoUpload.ContentType.ToLowerInvariant();

        // Validate file extension
        if (!allowedExtensions.Contains(fileExtension))
        {
            if (_photoRepo.GetType() == typeof(PhotoRepository))
            {
                TempData["Message"] = "Invalid file extension.";
                TempData["context"] = "danger";
            }
            return null;
        }

        // Validate MIME type (more robust)
        if (!allowedMimeTypes.Contains(mimeType))
        {
            return null; // Invalid MIME type
        }

        const long maxFileSize = 10 * 1024 * 1024; // 10 MB
        if (photoUpload.Length > maxFileSize)
        {
            TempData["Message"] = "The file size is too large.";
            TempData["context"] = "danger";
            return null; // File too large
        }

        // If all validations pass, proceed with saving the file
        string productPhotosFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + photoFolder);
        Directory.CreateDirectory(productPhotosFolder);

        string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(photoUpload.FileName);
        string filePath = Path.Combine(productPhotosFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await photoUpload.CopyToAsync(stream);
        }

        return new File { FileName = "/" + photoFolder + "/" + uniqueFileName };
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

        // Attempt to save a new photo
        File? uploadedImage = null;
        if (viewModel.PhotoUpload != null)
        {
            uploadedImage = await SaveImageAsync(viewModel.PhotoUpload, "productPhotos");
            if (uploadedImage == null)
            {
                ModelState.AddModelError("PhotoUpload", "Invalid image file. Only JPG, JPEG, PNG, or WebP images up to 5MB are allowed.");
            }
        }

        if (!ModelState.IsValid)
        {
            // Repopulate dropdown and checkbox on error
            var tags = await _productRepo.GetAllProductTagsAsync();
            var types = await _productRepo.GetAllProductTypesAsync();
            viewModel.Types = new SelectList(types, "TypeId", "Type", viewModel.SelectedTypeId);
            viewModel.Tags = tags.Select(tag => new SelectListItem
            {
                Value = tag.TagID.ToString(),
                Text = tag.Tag,
                Selected = viewModel.SelectedTagIds != null && viewModel.SelectedTagIds.Contains(tag.TagID)
            }).ToList();

            TempData["Message"] = "There were data-entry errors. Please check the form.";
            TempData["context"] = "danger";
            return View(viewModel);
        }

        Product productToUpdate;

        if (viewModel.ProductId == 0) // This is a new product
        {
            productToUpdate = new Product();
            TempData["Message"] = "Element successfully added.";
        }
        else // This is an existing product
        {
            // Load the existing product including its current relationships
            productToUpdate = await _productRepo.GetProductByIdAsync(viewModel.ProductId);

            if (productToUpdate == null)
            {
                TempData["Message"] = "Product not found for update.";
                TempData["context"] = "danger";
                return NotFound();
            }
            if (_productRepo.GetType() == typeof(ProductRepository))
            {
                TempData["Message"] = "Element successfully updated.";
            }
        }

        // Update scalar properties
        productToUpdate.ProductName = viewModel.ProductName;
        productToUpdate.ProductPrice = viewModel.ProductPrice;
        productToUpdate.ProductIngredients = viewModel.ProductIngredients;

        // Handle Product Photo
        if (uploadedImage != null)
        {
            productToUpdate.ProductPhoto = uploadedImage;
        }
        else if (viewModel.ProductId > 0 && viewModel.PhotoUpload == null) // If editing and no new photo uploaded, retain old one
        {
            // If it's an existing product and no new photo was uploaded,
            // we don't need to do anything with ProductPhoto, as it's already loaded.
            // If you were removing the photo, you'd set productToUpdate.ProductPhoto = new File(); or null;
        }
        else if (viewModel.ProductId == 0 && uploadedImage == null) // If new product and no photo
        {
            productToUpdate.ProductPhoto = new File();
        }

        // Clear existing categories and add the new one
        productToUpdate.ProductCategory.Clear();
        var selectedProductType = await _productRepo.GetProductTypeByIdAsync(viewModel.SelectedTypeId);
        if (selectedProductType != null)
        {
            productToUpdate.ProductCategory.Add(selectedProductType);
        }
        else
        {
            ModelState.AddModelError("SelectedTypeId", "Invalid product type selected.");
            // Repopulate dropdowns, checkboxes, and return view if this happens
            var tags = await _productRepo.GetAllProductTagsAsync();
            var types = await _productRepo.GetAllProductTypesAsync();
            viewModel.Types = new SelectList(types, "TypeId", "Type", viewModel.SelectedTypeId);
            viewModel.Tags = tags.Select(tag => new SelectListItem
            {
                Value = tag.TagID.ToString(),
                Text = tag.Tag,
                Selected = viewModel.SelectedTagIds != null && viewModel.SelectedTagIds.Contains(tag.TagID)
            }).ToList();
            if (_productRepo.GetType() == typeof(ProductRepository))    // otherwise unit tests fail
            {
                TempData["Message"] = "Invalid product type. Please check the form.";
                TempData["context"] = "danger";
            }
            return View(viewModel);
        }

        // Handle Product Tags (Many-to-many)
        // First, clear all existing tags for this product
        productToUpdate.Tags.Clear();

        // Then, add the newly selected tags
        if (viewModel.SelectedTagIds != null && viewModel.SelectedTagIds.Any())
        {
            var selectedTags = await _productRepo.GetTagsByIdsAsync(viewModel.SelectedTagIds);
            foreach (var tag in selectedTags)
            {
                productToUpdate.Tags.Add(tag);
            }
        }

        if (viewModel.ProductId == 0)
        {
            await _productRepo.AddProductAsync(productToUpdate);
        }
        else
        {
            await _productRepo.UpdateProductAsync(productToUpdate);
        }
        if (_productRepo.GetType() == typeof(ProductRepository))    // otherwise unit tests fail
        {
            TempData["context"] = "success";
        }
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

    public async Task<IActionResult> AdminApplications()
    {
        AdminApplicationsViewModel viewModel = new AdminApplicationsViewModel
        {
            Applications = await _applicationRepo.GetAllApplicationsAsync(),
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadResume(int id)
    {
        //get related resume
        var application = await _context.Applications
        .Include(a => a.ResumeFile)
        .FirstOrDefaultAsync(a => a.ApplicationID == id);

        //confirm fetched resume
        if (application == null || application.ResumeFile == null)
        {
            Console.WriteLine("Application/Resume not found");
            return NotFound();
        }

        //build path
        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        string filePath = Path.Combine(uploadsFolder, application.ResumeFile.FileName);
        string normalizedFilePath = filePath.Replace("/", "\\");

        if (!System.IO.File.Exists(normalizedFilePath))
        {
            Console.WriteLine("Resume file not found");
            return NotFound();
        }

        //configure return file type
        byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(normalizedFilePath);
        string contentType = string.IsNullOrEmpty(application.ResumeFile.ContentType)
            ? "application/octet-stream" : application.ResumeFile.ContentType;

        return File(fileBytes, contentType, Path.GetFileName(filePath));
    }
}