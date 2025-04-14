using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; } = "";

        [Required]
        public int PhoneNumber { get; set; }

        [RegularExpression(@"^[A-Za-z0-9\.]+@[A-Za-z0-9\.]+$")]
        [StringLength(50)]
        [Required]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(50)] public string Subject { get; set; } = "";

        [Required]
        public string MessageText { get; set; } = "";

        //uncomment when AppUser is added
        //public AppUser? Customer { get; set; }
    }
}
