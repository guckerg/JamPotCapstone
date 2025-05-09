using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }
        [StringLength(50)] public string ProductName { get; set; } = "";
        public decimal ProductPrice { get; set; }
        [StringLength(255)]
        public string ProductIngredients { get; set; } = "";
    }
}
