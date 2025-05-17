using JampotCapstone.Models;

namespace JampotCapstone.Data.Interfaces;

public interface ITextElementRepository
{
    public Task<List<TextElement>> GetAllTextElementsAsync();
    public Task<List<TextElement>> GetTextElementsByPageAsync(string page);
    public Task<TextElement> GetTextElementByPageAsync(string page);
    
    public Task<TextElement> GetTextElementByIdAsync(int id);
    
    public Task<int> StoreTextElementAsync(TextElement textblock);
    
    public Task<int> UpdateTextElementAsync(TextElement textblock);
    
    public Task<int> DeleteTextElementAsync(TextElement textblock);
    
}