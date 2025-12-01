using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using BusBookingApp.Controllers;
using BusBookingApp.Services;
using BusBookingApp.DTOs;

namespace BusBookingApp.Tests
{
    public class SearchControllerTests
    {
        [Fact]
        public async Task SearchBusesGet_ReturnsOkWithResults()
        {
            var mockService = new Mock<ISearchService>();
            var mockLogger = new Mock<ILogger<SearchController>>();

            mockService
                .Setup(s => s.SearchBusesAsync(It.IsAny<SearchBusDto>()))
                .ReturnsAsync(new List<BusScheduleDto>
                {
                    new BusScheduleDto { ScheduleId = 1, BusId = 1, BusNumber = "BUS1", Origin = "A", Destination = "B", DepartureTime = DateTime.UtcNow.AddHours(2), ArrivalTime = DateTime.UtcNow.AddHours(5), Price = 10, AvailableSeats = 10 }
                });

            var controller = new SearchController(mockService.Object, mockLogger.Object);

            var travelDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var result = await controller.SearchBusesGet("A", "B", travelDate);

            Assert.IsType<OkObjectResult>(result.Result);

            var ok = result.Result as OkObjectResult;
            var items = Assert.IsAssignableFrom<IEnumerable<BusScheduleDto>>(ok!.Value);
            Assert.Single(items);
        }

        [Fact]
        public async Task SearchBusesGet_ReturnsNotFoundWhenNoResults()
        {
            var mockService = new Mock<ISearchService>();
            var mockLogger = new Mock<ILogger<SearchController>>();

            mockService
                .Setup(s => s.SearchBusesAsync(It.IsAny<SearchBusDto>()))
                .ReturnsAsync(new List<BusScheduleDto>());

            var controller = new SearchController(mockService.Object, mockLogger.Object);

            var travelDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var result = await controller.SearchBusesGet("A", "B", travelDate);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task SearchBusesGet_ReturnsBadRequestForInvalidInput()
        {
            var mockService = new Mock<ISearchService>();
            var mockLogger = new Mock<ILogger<SearchController>>();

            var controller = new SearchController(mockService.Object, mockLogger.Object);

            var travelDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var result = await controller.SearchBusesGet("", "B", travelDate);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
