using System.ComponentModel.DataAnnotations;

namespace BusBookingApp.Models
{
    public class Bus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string BusNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string BusType { get; set; } = string.Empty; // AC, Non-AC, Sleeper, Seater

        [Required]
        public int TotalSeats { get; set; }

        [MaxLength(500)]
        public string? Amenities { get; set; } // WiFi, Charging, Water, etc.

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}