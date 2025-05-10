namespace JampotCapstone.Models;

public class File
{
    public int FileID { get; set; }
    public string ContentType { get; set; } = ""; //(e.g., application/pdf)
    public string FileName { get; set; } = ""; //To store the original file name

    public List<Page> Pages { get; set; } =
        new List<Page>(); // To correlate a photo with the page it should be displayed on
}
