using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Data;

public class PagePositionRepository : IPagePositionRepository
{
    private readonly ApplicationDbContext _context;

    public PagePositionRepository(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    
    public async Task<PagePosition> GetPagePosition(int pageId, int fileId)
    {
        PagePosition? model = await _context.PagePositions
            .FirstOrDefaultAsync(p => p.PageId == pageId && p.FileId == fileId);
        return model;
    }

    public async Task<int> UpdatePagePosition(PagePosition pagePosition)
    {
        _context.PagePositions.Update(pagePosition);
        return await _context.SaveChangesAsync();
    }

    public Task<int> StorePagePosition(PagePosition pos)
    {
        throw new NotImplementedException();
    }
}