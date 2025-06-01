using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace JampotCapstone.Controllers
{
    public class CareersController : Controller
    {
        IApplicationRepository repo;
        private ApplicationDbContext _context;
        private ITextElementRepository _textRepo;

        public CareersController(IApplicationRepository r, ApplicationDbContext c, ITextElementRepository t)
        {
            repo = r;
            _context = c;
            _textRepo = t;
        }

        public async Task<IActionResult> Index()
        {
            //create a list of jobtitle objects currently available
            var positions = _context.JobTitles.Select(j => new { j.JobTitleID, j.JobTitleName }).ToList();

            //populate viewModel passing positions list for dropdown menu
            var viewModel = new CareersViewModel
            {
                Positions = new SelectList(positions, "JobTitleID", "JobTitleName"),
                Application = new Application()
            };
            
            ViewBag.Text =
                await _textRepo.GetTextElementByPageAsync("careers");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApplication(CareersViewModel viewModel)
        {
            IFormFile Resume = viewModel.ResumeUpload;

            if (!ModelState.IsValid)
            {
                viewModel.Positions = new SelectList(_context.JobTitles, "JobTitleID", "JobTitleName");
                return View("Index", viewModel);
            }

            // If a resume file was uploaded...
            if (Resume != null && Resume.Length > 0)
            {
                //Validate the file extension.
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(Resume.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("Resume", "Only .pdf, .doc, and .docx files are allowed.");
                    viewModel.Positions = new SelectList(_context.JobTitles, "JobTitleID", "JobTitleName");
                    return View("Index", viewModel);
                }

                
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                //Ensure the uploads folder exists.
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                //Save the file to disk.
                var filePath = Path.Combine(uploadFolder, Resume.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Resume.CopyToAsync(stream);
                }

                // Create a relative path for storing in the database.
                var storedFileName = Resume.FileName;


                // Save the file metadata using your custom File class.
                var file = new Models.File
                {
                    FileName = storedFileName,
                    ContentType = Resume.ContentType,
                };
                _context.Files.Add(file);
                await _context.SaveChangesAsync();


                //Link the uploaded file to the application.
                viewModel.Application.ResumeFileID = file.FileID;
                viewModel.Application.ResumeFile = file;
            }

            var application = viewModel.Application;
            await repo.AddApplicationAsync(application);
            TempData["SuccessMessage"] = "Your application has been submitted successfully!";
            return RedirectToAction("Index", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApplication(int id)
        {
            repo.DeleteApplication(id);
            TempData["SuccessMessage"] = "The application has been deleted successfully!";
            return RedirectToAction("AdminApplications", "Admin");
        }
    }
}
