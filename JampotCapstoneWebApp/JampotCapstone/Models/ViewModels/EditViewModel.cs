namespace JampotCapstone.Models.ViewModels;

public class EditViewModel
{
    public int Position { get; set; } = 0;
    public List<Page> Pages { get; set; } = new List<Page>();
    public string Key { get; set; } = "";
    public int Page { get; set; } = 0;
}