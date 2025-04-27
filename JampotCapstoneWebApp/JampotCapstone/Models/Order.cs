using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class Order
    {
        public int OrderId {  get; set; }  
        public int SubTotal { get; set; }

        // Might need this to keep track of names and order, but it would be in the checkout process.
        //[StringLength(50), Required]
        //public string CustomerName { get; set; } = "";
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
