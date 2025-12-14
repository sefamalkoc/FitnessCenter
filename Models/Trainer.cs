using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Trainer name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Availability { get; set; } // Description of working hours, e.g. "Mon-Fri 09:00-17:00"

        // Many-to-Many relationship with Service
        public virtual ICollection<Service> Specialties { get; set; } = new List<Service>();
        
        // Navigation for Appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
