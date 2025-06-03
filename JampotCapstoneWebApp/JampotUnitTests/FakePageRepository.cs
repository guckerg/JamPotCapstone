namespace JampotCapstone.Data;

public class FakePageRepository : IPageRepository
{
    private List<Page> _pages = [];
    
    public async Task<List<Page>> GetAllPagesAsync()
    {
        return _pages;
    }

    public async Task<List<Page>> GetNonEmptyPagesAsync()
    {
        return _pages.Where(p => p.Files.Count > 0).ToList();
    }

    public async Task<Page> GetPageByIdAsync(int id)
    {
        Page? model = _pages.Find(p => p.PageId == id);
        return model;
    }

    public async Task<Page> GetPageByNameAsync(string name)
    {
        Page? model = _pages.Find(p => p.PageTitle.ToLower().Contains(name.ToLower()));
        return model;
    }

    public async Task<int> UpdatePageAsync(Page page)
    {
        int result = 0;
        int index = page.PageId - 1;
        if (_pages[index] == page)
        {
            _pages[index] = page;
            result = 1;
        }
        return result;
    }

    public async Task<int> StorePageAsync(Page page)
    {
        int result = 0;
        if(page != null)
        {
            _pages.Add(page);
            _pages[_pages.Count - 1].PageId = _pages.Count;
            result = 1;
        }
        return result;
    }
}