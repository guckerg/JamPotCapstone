using JampotCapstone.Models;
namespace JampotCapstone.Data;

public interface ITextElementRepository
{
    public Task<List<TextElement>> GetAllTextElements();
    public Task<List<TextElement>> GetTextElementsByPage(string page);
    public Task<TextElement> GetTextElementByPage(string page);
    
    public Task<TextElement> GetTextElementById(int id);
    
    public Task<int> CreateTextElement(TextElement textblock);
    
    public Task<int> UpdateTextElement(TextElement textblock);
    
    public Task<int> DeleteTextElement(TextElement textblock);
    
}