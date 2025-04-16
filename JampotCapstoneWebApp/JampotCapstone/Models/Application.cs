using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        [RegularExpression(@"^[A-Za-z0-9\.]+@[A-Za-z0-9\.]+$")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public File Resume { get; set; }

        [Required]
        public string Position { get; set; }

        public string Question1 { get; set; } = string.Empty;

        public string Question2 { get; set; } = string.Empty;

        //May use if client decided to have accounts for users.
        //public AppUser Applicant { get; set; }
    }
}
