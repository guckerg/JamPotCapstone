using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CareersController : Controller
    {
        IApplicationRepository repo;

        public CareersController(IApplicationRepository r)
        {
            repo = r;
        }

        public IActionResult Index()
        {
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
