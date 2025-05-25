using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Models;
public class PagePosition
{
    public int PagePositionId { get; set; }
    public int PageId { get; set; }
    public int FileId { get; set; }
    public int Position { get; set; }
}