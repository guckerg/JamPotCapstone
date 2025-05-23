using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models;

public class Product
{
    public int ProductId { get; set; }
    [StringLength(50)] public string ProductName { get; set; } = "";
    public decimal ProductPrice { get; set; }
    [StringLength(255)]
    public string ProductIngredients { get; set; } = "";
    public File ProductPhoto { get; set; } = new File();
    public ICollection<ProductType> ProductCategory { get; set; } = new List<ProductType>(); // Initialize to avoid null errors
    public ICollection<ProductTag> Tags { get; set; } = new List<ProductTag>(); // Initialize
}