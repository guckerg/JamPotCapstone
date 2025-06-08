using System.ComponentModel.DataAnnotations;

namespace JampotCapstone.Models
{
    public class JobTitle
    {
        [Key]
        public int JobTitleID { get; set; }

        public string JobTitleName { get; set; } = string.Empty;
    }
}
