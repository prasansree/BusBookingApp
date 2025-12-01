using System.ComponentModel.DataAnnotations;

namespace BusBookingApp.Models
{
    public class Routes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Origin { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public decimal Distance { get; set; } // in kilometers

        [Required]
        public int Duration { get; set; } // in minutes

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}