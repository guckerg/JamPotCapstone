using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace JampotCapstone.Models
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }

        [StringLength(255, ErrorMessage = "Name must be 255 characters or fewer.")]
        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; }
      
        [Required(ErrorMessage = "Please enter your phone number.")]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Please enter a valid phone number, such as (123) 456-7890 or 123-456-7890.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
        [StringLength(50, ErrorMessage = "Email must be 50 characters or fewer.")]
        [Required(ErrorMessage = "Please enter your email.")]
        public string Email { get; set; } = string.Empty;
      
        [Required]
        public int JobTitleID { get; set; }

        public JobTitle? JobTitle { get; set; }

        [Required(ErrorMessage = "Please answer this question.")]
        public string Question1 { get; set; } = string.Empty;
      
        [Required(ErrorMessage = "Please answer this question.")]
        public string Question2 { get; set; } = string.Empty;

        public int? ResumeFileID { get; set; } //nullable initially

        [ValidateNever]
        public File ResumeFile { get; set; } //navigation prop
    }
}
