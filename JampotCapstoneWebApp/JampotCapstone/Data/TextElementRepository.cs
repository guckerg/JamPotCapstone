using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Data;

public class TextElementRepository : ITextElementRepository
{
    private ApplicationDbContext _context;

    public TextElementRepository(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    
    public async Task<List<TextElement>> GetAllTextElementsAsync()
    {
        List<TextElement> model = await _context.TextElements
            .Include(t => t.Page).ToListAsync();
        return model;
    }

    public async Task<List<TextElement>> GetTextElementsByPageAsync(string page)
    {
        int pageId = _context.Pages.FirstOrDefaultAsync(p => p.PageTitle.ToLower().Contains(page.ToLower())).Result.PageId;
        List<TextElement> model = await _context.TextElements
            .Where(t => t.PageId == pageId)
            .ToListAsync();
        return model;
    }

    public async Task<TextElement> GetTextElementByPageAsync(string page)
    {
        int pageId = _context.Pages.FirstOrDefaultAsync(p => p.PageTitle.ToLower().Contains(page.ToLower())).Result.PageId;
        TextElement model = await _context.TextElements
            .FirstOrDefaultAsync(t => t.PageId == pageId);
        return model;
    }

    public async Task<TextElement> GetTextElementByIdAsync(int id)
    {
        TextElement? model = await _context.TextElements.FindAsync(id);
        return model;
    }

    public Task<int> StoreTextElementAsync(TextElement textblock)
    {
        _context.TextElements.Add(textblock);
        return _context.SaveChangesAsync();
    }

    public Task<int> UpdateTextElementAsync(TextElement textblock)
    {
        _context.TextElements.Update(textblock);
        return _context.SaveChangesAsync();
    }

    public Task<int> DeleteTextElementAsync(TextElement textblock)
    {
        _context.TextElements.Remove(textblock);
        return _context.SaveChangesAsync();
    }
}