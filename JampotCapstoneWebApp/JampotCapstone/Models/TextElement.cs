namespace JampotCapstone.Models;

public class TextElement
{
    public int TextElementId { get; set; }
    public string Name { get; set; } = "";
    public string Content { get; set; } = "";
    public Page? Page { get; set; } = new Page ();
    public int PageId { get; set; }
}