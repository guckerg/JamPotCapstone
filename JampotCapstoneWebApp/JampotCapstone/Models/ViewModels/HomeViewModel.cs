namespace JampotCapstone.Models.ViewModels;

public class HomeViewModel
{
    public File Map { get; set; } = new File();
    public File Special { get; set; } = new File();
    public List<File> Photos { get; set; } = new List<File>();
}