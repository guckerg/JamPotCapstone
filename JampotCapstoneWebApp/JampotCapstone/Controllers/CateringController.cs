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
        private ITextElementRepository _repo;

        public CateringController(ApplicationDbContext ctx, ITextElementRepository r)
        {
            _context = ctx;
            _repo = r;
        }
        public async Task<IActionResult> Index()
        {
            
            Page currentPage = _context.Pages
                .Where(p => p.PageTitle.ToLower().Contains("catering"))
                .Include(p => p.Files).FirstOrDefault();
            CateringViewModel model = new CateringViewModel
            {
                Textblocks = await _repo.GetTextElementsByPage("catering"),
                Photos = currentPage == null ? [] : currentPage.Files
            };
            return View(model);
        }
    }
}
