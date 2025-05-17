using JampotCapstone.Models;
namespace JampotCapstone.Data;


public interface IPageRepository
{
    public Task<List<Page>> GetAllPagesAsync();
    public Task<List<Page>> GetNonEmptyPagesAsync();
    public Task<Page> GetPageByIdAsync(int id);
    
    public Task<Page> GetPageByNameAsync(string name);
    
    public Task<int> UpdatePageAsync(Page page);
}