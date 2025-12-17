using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        public string Name { get; set; } = string.Empty; // Yoga, Pilates, etc.

        [Required]
        [Range(1, 480, ErrorMessage = "Süre 1 ile 480 dakika arasında olmalıdır.")]
        public int DurationMinutes { get; set; } // Duration in minutes

        [Required]
        [DataType(DataType.Currency)]
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
    }
}
