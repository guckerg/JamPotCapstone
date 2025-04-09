using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JampotCapstone.Models;

public class AppUser : IdentityUser
{
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public DateTime SignUpDate { get; set; }
}