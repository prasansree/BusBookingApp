using Microsoft.EntityFrameworkCore;
using BusBookingApp.Models;

namespace BusBookingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Bus entity configuration
            modelBuilder.Entity<Bus>()
                .HasIndex(b => b.BusNumber)
                .IsUnique();

            // Route entity configuration
            modelBuilder.Entity<Routes>()
                .HasIndex(r => new { r.Origin, r.Destination });

            // Booking entity configuration
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingReference)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data (optional)
           // SeedData(modelBuilder);
        }

        

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin User",
                    Email = "admin@busbooking.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Phone = "1234567890",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed sample routes
            modelBuilder.Entity<Routes>().HasData(
                new Routes { Id = 1, Origin = "New York", Destination = "Boston", Distance = 215, Duration = 240, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Routes { Id = 2, Origin = "Los Angeles", Destination = "San Francisco", Distance = 380, Duration = 360, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Routes { Id = 3, Origin = "Chicago", Destination = "Detroit", Distance = 280, Duration = 300, IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Seed sample buses
            modelBuilder.Entity<Bus>().HasData(
                new Bus { Id = 1, BusNumber = "BUS001", BusType = "AC Sleeper", TotalSeats = 40, Amenities = "WiFi, Charging, Water", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Bus { Id = 2, BusNumber = "BUS002", BusType = "Non-AC Seater", TotalSeats = 50, Amenities = "Water", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Bus { Id = 3, BusNumber = "BUS003", BusType = "AC Seater", TotalSeats = 45, Amenities = "WiFi, Charging", IsActive = true, CreatedAt = DateTime.UtcNow }
            );
        }
    }
}