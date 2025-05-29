using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [StringLength(255, ErrorMessage = "Name must be 255 characters or fewer.")] 
        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Please enter your phone number.")]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Please enter a valid phone number, such as (123) 456-7890 or 123-456-7890.")]
        public string PhoneNumber { get; set; } = "";

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
        [StringLength(50, ErrorMessage = "Email must be 50 characters or fewer.")]
        [Required(ErrorMessage = "Please enter your email.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Please enter a subject.")]
        [StringLength(50, ErrorMessage = "Subject must be 50 characters or fewer.")] 
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Please enter a message.")]
        public string MessageText { get; set; } = "";
    }
}
