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
    
    public async Task<List<TextElement>> GetAllTextElements()
    {
        List<TextElement> model = await _context.TextElements.ToListAsync();
        return model;
    }

    public async Task<List<TextElement>> GetTextElementsByPage(string page)
    {
        List<TextElement> model = await _context.TextElements.Where(t => t.Page.PageTitle == page).ToListAsync();
        return model;
    }

    public async Task<TextElement> GetTextElementByPage(string page)
    {
        TextElement model = await _context.TextElements.FirstOrDefaultAsync(t => t.Page.PageTitle == page);
        return model;
    }

    public async Task<TextElement> GetTextElementById(int id)
    {
        TextElement? model = await _context.TextElements.FindAsync(id);
        return model;
    }

    public Task<int> CreateTextElement(TextElement textblock)
    {
        _context.TextElements.Add(textblock);
        return _context.SaveChangesAsync();
    }

    public Task<int> UpdateTextElement(TextElement textblock)
    {
        _context.TextElements.Update(textblock);
        return _context.SaveChangesAsync();
    }

    public Task<int> DeleteTextElement(TextElement textblock)
    {
        _context.TextElements.Remove(textblock);
        return _context.SaveChangesAsync();
    }
}