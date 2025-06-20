using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace JampotUnitTests;

public class FakePagePositionRepository : IPagePositionRepository
{
    private readonly List<PagePosition> _positions = [];
    
    public async Task<PagePosition> GetPagePosition(int pageId, int fileId)
    {
        PagePosition? model = _positions.Find(p => p.PageId == pageId 
                                                   && p.FileId == fileId);
        return model;
    }

    public async Task<int> UpdatePagePosition(PagePosition pagePosition)
    {
        int result = 0;
        int index = pagePosition.PagePositionId - 1;
        if (_positions[index] == pagePosition)
        {
            _positions[index] = pagePosition;
            result = 1;
        }
        return result;
    }

    public async Task<int> StorePagePosition(PagePosition pos)
    {
        int result = 0;
        if(pos != null)
        {
            _positions.Add(pos);
            _positions[_positions.Count - 1].PagePositionId = _positions.Count;
            result = 1;
        }
        return result;
    }
}