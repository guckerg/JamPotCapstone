using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; } = "";
        [Required]
        [Range(0.01, 9999.99)]
        public decimal ProductPrice { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductIngredients { get; set; } = "";
        public List<int>? SelectedTagIds { get; set; }
        public int SelectedTypeId { get; set; }
        public List<SelectListItem> Tags { get; set; } = new List<SelectListItem>();
        public SelectList Types { get; set; } = new SelectList(Enumerable.Empty<string>(), "TypeId", "Type");
        public IFormFile? PhotoUpload { get; set; } // optional photo upload
    }
}
