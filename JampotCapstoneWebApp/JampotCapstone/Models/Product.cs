using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models;

public class Product
{
    public int ProductId { get; set; }
    [StringLength(50)] public string ProductName { get; set; } = "";
    public decimal ProductPrice { get; set; }
    public string ProductIngredients { get; set; } = "";
    public File ProductPhoto { get; set; } = new File();
    public ICollection<ProductType> ProductCategory { get; set; } = new List<ProductType>();
    public ICollection<ProductTag>? Tags { get; set; }
}