namespace JampotCapstone.Models;

public class Page
{
    public int PageId { get; set; }
    public string PageTitle { get; set; } = "";
    public List<PagePosition> Files { get; set; } = [];
}