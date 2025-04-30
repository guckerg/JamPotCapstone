namespace JampotCapstone.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal TotalPrice => Items.Sum(i => i.LineTotal);
        public int TotalQuantity => Items.Sum(i => i.Quantity);
    }

}
