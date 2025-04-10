using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class ProductType
    {
        [Key]
        public int TypeId { get; set; }
        public String Type { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
