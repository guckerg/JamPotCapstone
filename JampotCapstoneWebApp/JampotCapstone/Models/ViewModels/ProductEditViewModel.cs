using Microsoft.AspNetCore.Mvc.Rendering;
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
        public SelectList Tags { get; set; } = new SelectList(Enumerable.Empty<string>(), "TagID", "Tag");
        public SelectList Types { get; set; } = new SelectList(Enumerable.Empty<string>(), "TypeId", "Type");
        public IFormFile PhotoUpload { get; set; }



    }
}
