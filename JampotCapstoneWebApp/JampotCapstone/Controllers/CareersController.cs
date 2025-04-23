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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApplicationAsync(Application model)
        {

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            await repo.CreateApplicationAsync(model);
            return RedirectToAction("Index");
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
