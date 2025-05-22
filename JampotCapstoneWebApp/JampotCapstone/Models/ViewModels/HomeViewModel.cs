namespace JampotCapstone.Models.ViewModels;

public class HomeViewModel
{
    public TextElement Hours { get; set; } = new TextElement();
    public List<File> Photos { get; set; } = new List<File>();
}