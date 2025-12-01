using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using BusBookingApp.Controllers;
using BusBookingApp.Data;
using BusBookingApp.Models;
using BusBookingApp.Services;
using BusBookingApp.DTOs;

namespace BusBookingApp.Tests
{
    public class AuthControllerTests
    {
        private ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithValidCredentials()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var password = "Secret123!";
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Phone = "1234567890",
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("mock-token");

            var controller = new AuthController(context, jwtMock.Object);

            var loginDto = new LoginDto { Email = user.Email, Password = password };
            var action = await controller.Login(loginDto);

            Assert.IsType<OkObjectResult>(action.Result);

            var ok = action.Result as OkObjectResult;
            var response = Assert.IsType<AuthResponseDto>(ok!.Value);
            Assert.Equal("mock-token", response.Token);
            Assert.Equal(user.Email, response.Email);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailExists()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var existing = new User
            {
                Name = "Existing",
                Email = "exists@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("p"),
                Phone = "000",
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            context.Users.Add(existing);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            var controller = new AuthController(context, jwtMock.Object);

            var registerDto = new RegisterDto { Name = "New", Email = existing.Email, Password = "pw", Phone = "111" };
            var action = await controller.Register(registerDto);

            Assert.IsType<BadRequestObjectResult>(action.Result);
        }

        [Fact]
        public async Task Register_CreatesUser_AndReturnsCreated()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("reg-token");

            var controller = new AuthController(context, jwtMock.Object);

            var registerDto = new RegisterDto { Name = "New", Email = "new@example.com", Password = "pw123", Phone = "111" };
            var action = await controller.Register(registerDto);

            Assert.IsType<CreatedAtActionResult>(action.Result);

            var created = action.Result as CreatedAtActionResult;
            var response = Assert.IsType<AuthResponseDto>(created!.Value);
            Assert.Equal("reg-token", response.Token);
            Assert.Equal(registerDto.Email, response.Email);
        }
    }
}
