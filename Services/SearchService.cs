using BusBookingApp.Data;
using BusBookingApp.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusBookingApp.Services
{
    public class SearchService: ISearchService
    {
         private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchService> _logger;

        public SearchService(ApplicationDbContext context, ILogger<SearchService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<string>> GetBusLocationsAsync()
        {
            var origins = await _context.Routes.Select(r => r.Origin).Distinct().ToListAsync();
   

            var destinations = await _context.Routes
                .Select(r => r.Destination)
                .Distinct()
                .ToListAsync();

            var locations = origins.Union(destinations).ToList();

            return locations;
        }


        public async Task<List<BusScheduleDto>> SearchBusesAsync(SearchBusDto searchBusDto)
        {
            var results = await (from schedule in _context.Schedules
                                 join bus in _context.Buses on schedule.BusId equals bus.Id
                                 join route in _context.Routes on schedule.RouteId equals route.Id
                                 where route.Origin == searchBusDto.Origin
                                       && route.Destination == searchBusDto.Destination
                                       && DateOnly.FromDateTime(schedule.DepartureTime) == searchBusDto.TravelDate                                       
                                 select new BusScheduleDto
                                 {
                                     BusId = bus.Id,
                                     BusNumber = bus.BusNumber,
                                     BusType = bus.BusType,
                                     Origin = route.Origin,
                                     Destination = route.Destination,
                                     DepartureTime = schedule.DepartureTime,
                                     ArrivalTime = schedule.ArrivalTime,
                                     AvailableSeats = schedule.AvailableSeats,
                                     Price = schedule.Price
                                 }).ToListAsync();

            return results;
        }
        
    }
}