using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusBookingApp.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BusId { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime TravelDate { get; set; }

        public int AvailableSeats { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("BusId")]
        public virtual Bus Bus { get; set; } = null!;

        [ForeignKey("RouteId")]
        public virtual Routes Route { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}