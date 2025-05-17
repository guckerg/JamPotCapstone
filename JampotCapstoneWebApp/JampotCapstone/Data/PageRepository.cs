using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;
namespace JampotCapstone.Data;

public class PageRepository : IPageRepository
{
    private ApplicationDbContext _context;

    public PageRepository(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    public async Task<List<Page>> GetAllPagesAsync()
    {
        List<Page> pages = await _context.Pages.ToListAsync();
        return pages;
    }

    public async Task<List<Page>> GetNonEmptyPagesAsync()
    {
        List<Page> pages = await _context.Pages.Where(p => p.Files.Count > 0)
            .Include(p => p.Files).ToListAsync();
        return pages;
    }

    public async Task<Page> GetPageByIdAsync(int id)
    {
        Page? page = await _context.Pages
            .Include(p => p.Files)
            .FirstOrDefaultAsync(p => p.PageId == id);
        return page;
    }

    public async Task<Page> GetPageByNameAsync(string name)
    {
        Page? page = await _context.Pages
            .FirstOrDefaultAsync(p => p.PageTitle.ToLower().Contains(name.ToLower()));
        return page;
    }

    public async Task<int> UpdatePageAsync(Page page)
    {
        _context.Pages.Update(page);
        return await _context.SaveChangesAsync();
    }
}