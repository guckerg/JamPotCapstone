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
        [StringLength(255)]
        public string ProductIngredients { get; set; } = "";
        [Required]
        public int SelectedTagId { get; set; }
        [Required]
        public int SelectedTypeId { get; set; }
        public SelectList Tags { get; set; } = new SelectList(Enumerable.Empty<string>(), "TagID", "Tag");
        public SelectList Types { get; set; } = new SelectList(Enumerable.Empty<string>(), "TypeId", "Type");
        public File? PhotoUpload { get; set; } // optional photo upload
    }
}
