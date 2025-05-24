using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Models;
public class PagePosition
{
    /*public int Home { get; set; } = -1;
    public int Catering { get; set; } = -1;
    public int Menu { get; set; } = -1;
    public int About { get; set; } = -1;
    public int FAQs { get; set; } = -1;*/
    public int PagePositionId { get; set; }
    public int PageId { get; set; }
    public int FileId { get; set; }
    public int Position { get; set; }
}