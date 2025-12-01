
using BusBookingApp.DTOs;

namespace BusBookingApp.Services
{
    public interface ISearchService
    {
        Task<List<BusScheduleDto>> SearchBusesAsync(SearchBusDto searchBusDto);

        Task<List<string>> GetBusLocationsAsync();
    }
}