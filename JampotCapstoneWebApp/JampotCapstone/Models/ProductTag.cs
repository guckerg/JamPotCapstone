using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class ProductTag
    {
        [Key]
        public int TagID { get; set; }

        public string Tag { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
