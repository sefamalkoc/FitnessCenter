using Microsoft.AspNetCore.Identity;

namespace FitnessCenter.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional properties can be added here if needed
        public string? FullName { get; set; }
    }
}
