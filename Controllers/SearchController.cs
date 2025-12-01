using Microsoft.AspNetCore.Mvc;
using BusBookingApp.DTOs;
using BusBookingApp.Services;

namespace BusBookingApp.Controllers
{
    /// <summary>
    /// Controller for bus search operations
    /// All EF Core logic is in SearchService
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

  
        /// <summary>
        /// Search for available buses using GET (alternative)
        /// GET /api/search/buses?origin=NewYork&destination=Boston&travelDate=2025-12-01
        /// </summary>
        [HttpGet("buses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BusScheduleDto>>> SearchBusesGet(
            [FromQuery] string origin,
            [FromQuery] string destination,
            [FromQuery] DateOnly travelDate)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            {
                return BadRequest(new { message = "Origin and destination are required" });
            }

            if (travelDate < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return BadRequest(new { message = "Travel date cannot be in the past" });
            }

            try
            {
                var searchDto = new SearchBusDto
                {
                    Origin = origin,
                    Destination = destination,
                    TravelDate = travelDate
                };

                var results = await _searchService.SearchBusesAsync(searchDto);

                if (!results.Any())
                {
                    return NotFound(new 
                    { 
                        message = "No buses available for the selected route and date"
                    });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching buses");
                return StatusCode(500, new { message = "An error occurred while searching" });
            }
        }

        /// <summary>
        /// Get all unique bus locations (origins and destinations)     '
        /// </summary>
        /// <returns>List of unique bus locations</returns>
        /// <response code="200">Returns the list of bus locations</response>
        /// <response code="500">If an error occurs while retrieving locations</response>   
        /// <remarks>
        /// Sample request: GET /api/search/locations
        /// </remarks>
        [HttpGet("locations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<string>>> GetBusLocations()
        {
            try
            {
                var locations = await _searchService.GetBusLocationsAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bus locations");
                return StatusCode(500, new { message = "An error occurred while retrieving locations" });
            }
        }
    }
}