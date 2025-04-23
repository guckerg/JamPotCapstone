using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; } = "";

        [Required]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$")]
        public string PhoneNumber { get; set; } = "";

        [RegularExpression(@"^[A-Za-z0-9\.]+@[A-Za-z0-9\.]+$")]
        [StringLength(50)]
        [Required]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(50)] public string Subject { get; set; } = "";

        [Required]
        public string MessageText { get; set; } = "";
    }
}
