using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Tests
{
    public class GenericRepositoryTests
    {
        private ApplicationDbContext _context;
        private GenericRepository<TaskEntity> _taskRepository;
        private GenericRepository<ColumnEntity> _columnRepository;

        [SetUp]
        public void Setup()
        {
            // Set up in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _taskRepository = new GenericRepository<TaskEntity>(_context);
            _columnRepository = new GenericRepository<ColumnEntity>(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddEntity_WhenEntityIsValid()
        {
            // Arrange
            var task = new TaskEntity { Name = "Test Task", Status = "ToDo", ColumnId = 1 };

            // Act
            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            // Assert
            var addedTask = await _taskRepository.GetByIdAsync(task.Id);
            Assert.IsNotNull(addedTask);
            Assert.That(addedTask.Name, Is.EqualTo(task.Name));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities_WhenEntitiesExist()
        {
            // Arrange
            var task1 = new TaskEntity { Name = "Task 1", Status = "ToDo", ColumnId = 1 };
            var task2 = new TaskEntity { Name = "Task 2", Status = "InProgress", ColumnId = 2 };
            await _taskRepository.AddAsync(task1);
            await _taskRepository.AddAsync(task2);
            await _taskRepository.SaveChangesAsync();

            // Act
            var tasks = await _taskRepository.GetAllAsync();

            // Assert
            Assert.That(tasks.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            // Arrange
            var column = new ColumnEntity { Name = "ToDo" };
            await _columnRepository.AddAsync(column);
            await _columnRepository.SaveChangesAsync();

            // Act
            var fetchedColumn = await _columnRepository.GetByIdAsync(column.Id);

            // Assert
            Assert.IsNotNull(fetchedColumn);
            Assert.That(fetchedColumn.Name, Is.EqualTo(column.Name));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            // Act
            var nonExistentTask = await _taskRepository.GetByIdAsync(999);

            // Assert
            Assert.IsNull(nonExistentTask);
        }

        [Test]
        public async Task Update_ShouldUpdateEntity_WhenEntityExists()
        {
            // Arrange
            var task = new TaskEntity { Name = "Task to Update", Status = "ToDo", ColumnId = 1 };
            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            task.Name = "Updated Task Name";
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            // Act
            var updatedTask = await _taskRepository.GetByIdAsync(task.Id);

            // Assert
            Assert.IsNotNull(updatedTask);
            Assert.That(updatedTask.Name, Is.EqualTo("Updated Task Name"));
        }

        [Test]
        public async Task Delete_ShouldRemoveEntity_WhenEntityExists()
        {
            // Arrange
            var task = new TaskEntity { Name = "Task to Delete", Status = "ToDo", ColumnId = 1 };
            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            // Act
            _taskRepository.Delete(task);
            await _taskRepository.SaveChangesAsync();

            // Assert
            var deletedTask = await _taskRepository.GetByIdAsync(task.Id);
            Assert.IsNull(deletedTask);
        }

        [Test]
        public async Task RemoveAllAsync_ShouldRemoveAllEntities()
        {
            // Arrange
            var task1 = new TaskEntity { Name = "Task 1", Status = "ToDo", ColumnId = 1 };
            var task2 = new TaskEntity { Name = "Task 2", Status = "InProgress", ColumnId = 2 };
            await _taskRepository.AddAsync(task1);
            await _taskRepository.AddAsync(task2);
            await _taskRepository.SaveChangesAsync();

            // Act
            await _taskRepository.RemoveAllAsync();

            // Assert
            var tasks = await _taskRepository.GetAllAsync();
            Assert.That(tasks.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task SaveChangesAsync_ShouldSaveChanges_WhenCalled()
        {
            // Arrange
            var column = new ColumnEntity { Name = "New Column" };
            await _columnRepository.AddAsync(column);

            // Act
            await _columnRepository.SaveChangesAsync();
            var savedColumn = await _columnRepository.GetByIdAsync(column.Id);

            // Assert
            Assert.IsNotNull(savedColumn);
            Assert.That(savedColumn.Name, Is.EqualTo("New Column"));
        }

        [Test]
        public async Task AddAsync_ShouldAddColumnEntity_WhenEntityIsValid()
        {
            // Arrange
            var column = new ColumnEntity { Name = "New Column" };

            // Act
            await _columnRepository.AddAsync(column);
            await _columnRepository.SaveChangesAsync();

            // Assert
            var addedColumn = await _columnRepository.GetByIdAsync(column.Id);
            Assert.IsNotNull(addedColumn);
            Assert.That(addedColumn.Name, Is.EqualTo(column.Name));
        }
    }
}
