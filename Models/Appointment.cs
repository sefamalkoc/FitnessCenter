using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Approved,
        Cancelled,
        Completed
    }

    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // Foreign Keys
        [Required]
        public string MemberId { get; set; } = string.Empty;
        public virtual ApplicationUser? Member { get; set; }

        [Required]
        public int TrainerId { get; set; }
        public virtual Trainer? Trainer { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public virtual Service? Service { get; set; }
    }
}
