using JampotCapstone.Models;

namespace JampotCapstone.Data.Interfaces;

public interface IPagePositionRepository
{
    public Task<PagePosition> GetPagePosition(int pageId, int fileId);
    
    public Task<int> UpdatePagePosition(PagePosition pagePosition);
}