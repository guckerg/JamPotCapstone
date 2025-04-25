using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace JampotCapstone.Controllers
{
    public class CareersController : Controller
    {
        IApplicationRepository repo;
        private readonly ApplicationDbContext context;

        public CareersController(IApplicationRepository r, ApplicationDbContext c)
        {
            repo = r;
            context = c;
        }

        public IActionResult Index()
        {
            var positions = new SelectList(context.JobTitles, "JobTitleID", "JobTitleName");
            var model = new CareersViewModel
            {
                //Title = "Careers",
                Positions = positions,
                Application = new Application()
            };

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApplication(CareersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Positions = new SelectList(context.JobTitles, "JobTitleID", "JobTitleName");
                return View("Index", viewModel);
            }

            var application = viewModel.Application;
            await repo.AddApplicationAsync(application);
            TempData["SuccessMessage"] = "Your application has been submitted successfully!";
            return RedirectToAction("Index", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApplication(int ApplicationID)
        {
            repo.DeleteApplication(ApplicationID);
            return RedirectToAction("Index");
        }
    }
}
