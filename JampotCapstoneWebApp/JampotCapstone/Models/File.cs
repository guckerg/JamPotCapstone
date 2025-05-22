namespace JampotCapstone.Models;

public class File
{
    public int FileID { get; set; }
    public string ContentType { get; set; } = ""; //(e.g., application/pdf)
    public string FileName { get; set; } = ""; //To store the original file name

    public List<Page> Pages { get; set; } =
        new List<Page>(); // To correlate a photo with the page it should be displayed on

    public PagePosition PagePosition { get; set; } = new PagePosition();
    
    // get the filename out of the file path for user-friendly display
    public string GetFileName()
    {
        int startIndex = FileName.LastIndexOf("/");
        if (startIndex == -1)
        {
            startIndex = FileName.LastIndexOf("\\");
        }
        int endIndex = FileName.LastIndexOf(".");
        string fileName = FileName.Substring(startIndex, endIndex - startIndex);
        return fileName;
    }
}
