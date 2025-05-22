namespace JampotCapstone.Models;

public class PagePosition
{
    public int PagePositionId { get; set; }
    public int Home { get; set; } = -1;
    public int Catering { get; set; } = -1;
    public int Menu { get; set; } = -1;
    public int About { get; set; } = -1;
    public int FAQs { get; set; } = -1;
}