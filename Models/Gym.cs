using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class Gym
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Working hours are required")]
        public string WorkingHours { get; set; } = string.Empty; // e.g., "08:00-22:00"

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;
    }
}
