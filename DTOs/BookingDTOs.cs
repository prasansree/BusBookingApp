using System.ComponentModel.DataAnnotations;

namespace BusBookingApp.DTOs
{
    public class SearchBusDto
    {
        [Required]
        public string Origin { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateOnly TravelDate { get; set; }
    }

    public class BusScheduleDto
    {
        public int ScheduleId { get; set; }
        public int BusId { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public string BusType { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public int Duration { get; set; }
        public string? Amenities { get; set; }
    }

    public class PassengerDto
    {
        [Required]
        public string SeatNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;
    }

    public class CreateBookingDto
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public List<PassengerDto> Passengers { get; set; } = new();

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int NumberOfSeats { get; set; }
        public BusScheduleDto Schedule { get; set; } = null!;
        public List<PassengerDto> Passengers { get; set; } = new();
    }
}