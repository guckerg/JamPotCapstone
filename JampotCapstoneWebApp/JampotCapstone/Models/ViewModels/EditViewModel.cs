namespace JampotCapstone.Models.ViewModels;

public class EditViewModel
{
    public List<File> Photos { get; set; } = [];
    public string CurrentPage { get; set; } = "";
    public int OldPhotoId  { get; set; }
    public int NewPhotoId { get; set; }
}