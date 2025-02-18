using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagementApp.Controllers;
using TaskManagementApp.Data;
using TaskManagementApp.DTOs;
using TaskManagementApp.Models;
using TaskManagementApp.Services;

namespace TaskManagementApp.Tests
{
    public class TaskControllerTests
    {
        private Mock<IStorageService> _mockStorageService;
        private UnitOfWork _unitOfWork;
        private TaskController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Set up in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _unitOfWork = new UnitOfWork(_context);

            // Mock IStorageService
            _mockStorageService = new Mock<IStorageService>();
            _mockStorageService.Setup(service => service.UploadImageAsync(It.IsAny<System.IO.Stream>(), It.IsAny<string>()))
                .ReturnsAsync("http://example.com/uploadedImage.jpg");

            // Initialize controller with mocked services
            _controller = new TaskController(_unitOfWork, _mockStorageService.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateTask_ShouldReturnBadRequest_WhenColumnDoesNotExist()
        {
            // Arrange
            var request = new TaskCreateRequest
            {
                ColumnId = 999, // Column that does not exist
                UserId = 1,
                Name = "Test Task"
            };

            // Act
            var result = await _controller.CreateTask(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Column with Id 999 does not exist."));
        }

        [Test]
        public async Task CreateTask_ShouldReturnBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new TaskCreateRequest
            {
                ColumnId = 1,  // Assume this column exists
                UserId = 999,  // User that does not exist
                Name = "Test Task"
            };

            // Act
            var result = await _controller.CreateTask(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("User with Id 999 does not exist."));
        }

        [Test]
        public async Task CreateTask_ShouldReturnCreatedResult_WhenTaskIsValid()
        {
            // Arrange
            var column = new ColumnEntity { Id = 1, Name = "ToDo" };
            var user = new UserEntity { Id = 75, Username = "User1" };

            _context.Columns.Add(column);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new TaskCreateRequest
            {
                ColumnId = column.Id,
                UserId = user.Id,
                Name = "Test Task",
                Description = "This is a test task",
                Deadline = DateOnly.FromDateTime(System.DateTime.Now.AddDays(1)),
                IsFavoriteTask = false,
                Status = "ToDo"
            };

            // Act
            var result = await _controller.CreateTask(request);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.That(createdResult.ActionName, Is.EqualTo("GetTaskById"));
        }

        [Test]
        public async Task GetTaskById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _controller.GetTaskById(999); // Non-existent task

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange
            var task = new TaskEntity
            {
                Name = "Test Task",
                Description = "Test Description",
                Status = "ToDo",
                ColumnId = 1,
                UserId = 1
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTaskById(task.Id);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.Value, Is.EqualTo(task));
        }

        [Test]
        public async Task DeleteTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteTask(999); // Non-existent task

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteTask_ShouldReturnNoContent_WhenTaskDeleted()
        {
            // Arrange
            var task = new TaskEntity
            {
                Name = "Test Task",
                Description = "Test Description",
                Status = "ToDo",
                ColumnId = 1,
                UserId = 1
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteTask(task.Id);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
