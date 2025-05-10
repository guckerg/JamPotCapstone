using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CateringController : Controller
    {
        private ApplicationDbContext _context;

        public CateringController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public async Task<IActionResult> Index()
        { 
            CateringViewModel model = new CateringViewModel();
            model.Textblocks = await _context.TextElements
                .Where(t => t.Location.ToLower().Contains("catering")).ToListAsync();
            Page currentPage = _context.Pages.Where(p => p.PageTitle.ToLower().Contains("catering"))
                .Include(p => p.Files).FirstOrDefault();
            model.Photos = currentPage.Files;
            return View(model);
        }
    }
}
