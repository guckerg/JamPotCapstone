using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;

namespace JampotCapstone.Data;

public class FakeTextElementRepository : ITextElementRepository
{
    private List<TextElement> _textblocks = new List<TextElement>();
    
    public async Task<List<TextElement>> GetAllTextElementsAsync()
    {
        return _textblocks;
    }

    public async Task<List<TextElement>> GetTextElementsByPageAsync(string page)
    {
        List<TextElement> model = _textblocks
            .Where(t => t.Page.PageTitle.ToLower().Contains(page))
            .ToList();
        return model;
    }

    public async Task<TextElement> GetTextElementByPageAsync(string page)
    {
        TextElement model = _textblocks.Find(t => t.Page.PageTitle.ToLower().Contains(page));
        return model;
    }

    public async Task<TextElement> GetTextElementByIdAsync(int id)
    {
        TextElement? model = _textblocks.Find(t => t.TextElementId == id);
        return model;
    }

    public async Task<int> StoreTextElementAsync(TextElement textblock)
    {
        int result = 0;
        if(textblock != null && textblock.Page != null)
        {
            _textblocks.Add(textblock);
            _textblocks[_textblocks.Count - 1].TextElementId = _textblocks.Count;
            result = 1;
        }
        return result;
    }

    public async Task<int> UpdateTextElementAsync(TextElement textblock)
    {
        int result = 0;
        int index = textblock.TextElementId - 1;
        if (_textblocks[index] == textblock)
        {
            _textblocks[index] = textblock;
            result = 1;
        }
        return result;
    }

    public async Task<int> DeleteTextElementAsync(TextElement textblock)
    {
        int result = 0;
        if (_textblocks[textblock.TextElementId - 1] == textblock)
        {
            _textblocks.RemoveAt(textblock.TextElementId - 1);
            result = 1;
        }
        return result;
    }
}