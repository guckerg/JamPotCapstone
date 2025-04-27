namespace JampotCapstone.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public File ProductPhoto { get; set; } = new File();
        public int Quantity { get; set; }
        public decimal LineTotal => ProductPrice * Quantity;
    }
}
