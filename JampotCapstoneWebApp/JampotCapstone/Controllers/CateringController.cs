using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CateringController : Controller
    {
        private ITextElementRepository _repo;
        private IPhotoRepository _photoRepo;

        public CateringController(ITextElementRepository r, IPhotoRepository p)
        {
            _repo = r;
            _photoRepo = p;
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
