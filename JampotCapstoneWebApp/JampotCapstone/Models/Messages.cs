using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class Messages
    {
        public int MessageId { get; set; }
        [StringLength(50), Required]
        public string Sender { get; set; } = "";
        public int PhoneNumber { get; set; }

        [RegularExpression(@"^[A-Za-z0-9\.]+@[A-Za-z0-9\.]+$")]
        [StringLength(50)]
        public string Email { get; set; } = "";

        [StringLength(50)] public string Subject { get; set; } = "";
        public string MessageText { get; set; } = "";

        //uncomment when AppUser is added
        //public AppUser? Customer { get; set; }
    }
}
