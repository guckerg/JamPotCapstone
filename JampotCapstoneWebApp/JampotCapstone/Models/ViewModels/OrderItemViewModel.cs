namespace JampotCapstone.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public List<OrderItem> OrderItems { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? TotalQuantity { get; set; }
    }
}
