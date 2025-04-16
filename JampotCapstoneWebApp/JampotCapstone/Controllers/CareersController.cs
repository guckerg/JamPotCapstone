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
            this.context = c;
        }

        public IActionResult Index()
        {
            //ViewData["JobTitleID"] = new SelectList();

            return View();
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
