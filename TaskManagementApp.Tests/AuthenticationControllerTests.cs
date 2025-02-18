using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TaskManagementApp.Controllers;
using TaskManagementApp.Data;
using TaskManagementApp.DTOs;
using TaskManagementApp.Models;

namespace TaskManagementApp.Tests
{
    public class AuthenticationControllerTests
    {
        private AuthenticationController _authController;
        private ApplicationDbContext _context;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _mockConfiguration = new Mock<IConfiguration>();
            _authController = new AuthenticationController(_context, _mockConfiguration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUsernameExists()
        {
            // Arrange
            var userDto = new UserRequest
            {
                Username = "existinguser",
                Password = "password123",
                Role = "User"
            };

            // Simulate existing user in the database
            var existingUser = new UserEntity { Username = "existinguser", Password = "hashedPassword", Role = "User" };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authController.CreateUser(userDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Username already exists."));
        }

        [Test]
        public async Task CreateUser_ShouldReturnOk_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var userDto = new UserRequest
            {
                Username = "newuser",
                Password = "password123",
                Role = "User"
            };

            // Act
            var result = await _authController.CreateUser(userDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult!.StatusCode!, Is.EqualTo(200));

            // Check if the user was actually added to the database
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
            Assert.IsNotNull(createdUser);
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            // Arrange
            var userDto = new UserRequest
            {
                Username = "nonexistentuser",
                Password = "wrongpassword"
            };

            // Act
            var result = await _authController.Login(userDto);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.That(unauthorizedResult!.Value!, Is.EqualTo("Invalid username or password."));
        }

        [Test]
        public async Task Login_ShouldReturnOk_WhenValidCredentials()
        {
            // Arrange
            var userDto = new UserRequest
            {
                Username = "validuser",
                Password = "password123"
            };

            // Create a user in the database
            var hashedPassword = HashPassword(userDto.Password);
            var user = new UserEntity { Username = userDto.Username, Password = hashedPassword, Role = "User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authController.Login(userDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult!.StatusCode!, Is.EqualTo(200));
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
