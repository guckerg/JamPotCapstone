namespace JampotCapstone.Models.ViewModels;

public class OrderViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<ProductType> ProductTypes { get; set; }
}