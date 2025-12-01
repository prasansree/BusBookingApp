using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusBookingApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BookingReference { get; set; } = string.Empty;

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public int NumberOfSeats { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ScheduleId")]
        public virtual Schedule Schedule { get; set; } = null!;

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        
        public virtual Payment? Payment { get; set; }
    }
}