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
                .Where(t => t.PagePosition.Catering != -1)
                .OrderBy(t => t.PagePosition.Catering).ToListAsync();
            model.Photos = await _context.Files.Include(f => f.PagePosition)
                .Where(f => f.PagePosition.Catering != -1).OrderBy(f => f.PagePosition.Catering).ToListAsync();
            /*Page currentPage = _context.Pages.Where(p => p.PageTitle.ToLower().Contains("catering"))
                .Include(p => p.Files).FirstOrDefault();*/
            return View(model);
        }
    }
}
