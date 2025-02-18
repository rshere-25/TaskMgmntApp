using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Data;
using TaskManagementApp.DTOs;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using Serilog;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public TaskController(UnitOfWork unitOfWork, IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromForm] TaskCreateRequest request)
        {
            try
            {
                var column = await _unitOfWork.Columns.GetByIdAsync(request.ColumnId);
                if (column == null)
                {
                    Log.Warning("Task creation failed. Column with ID {ColumnId} does not exist.", request.ColumnId);
                    return BadRequest($"Column with Id {request.ColumnId} does not exist.");
                }

                var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    Log.Warning("Task creation failed. User with ID {UserId} does not exist.", request.UserId);
                    return BadRequest($"User with Id {request.UserId} does not exist.");
                }

                List<string> imageUrls = [];
                if (request.Files != null)
                {
                    foreach (var file in request.Files)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        using var stream = file.OpenReadStream();
                        var imageUrl = await _storageService.UploadImageAsync(stream, fileName);
                        imageUrls.Add(imageUrl);
                    }
                }

                var task = new TaskEntity
                {
                    Name = request.Name,
                    Description = request.Description,
                    Deadline = request.Deadline,
                    IsFavoriteTask = request.IsFavoriteTask,
                    Status = request.Status,
                    ColumnId = request.ColumnId,
                    ImageUrls = imageUrls,
                    UserId = request.UserId
                };

                await _unitOfWork.Tasks.AddAsync(task);
                await _unitOfWork.SaveChangesAsync();

                Log.Information("Task '{TaskName}' created successfully with ID {TaskId}.", task.Name, task.Id);
                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a task.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("GetTaskById")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null)
                {
                    Log.Warning("Task retrieval failed. Task with ID {TaskId} not found.", id);
                    return NotFound();
                }

                Log.Information("Task with ID {TaskId} retrieved successfully.", id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving task with ID {TaskId}.", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("GetTasks")]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks()
        {
            try
            {
                var tasks = await _unitOfWork.Tasks.GetAllAsync();
                Log.Information("Retrieved {TaskCount} tasks successfully.", tasks.Count());
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving tasks.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPost("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromForm] TaskCreateRequest request)
        {
            try
            {
                var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (existingTask == null)
                {
                    Log.Warning("Task update failed. Task with ID {TaskId} not found.", id);
                    return NotFound();
                }

                var column = await _unitOfWork.Columns.GetByIdAsync(request.ColumnId);
                if (column == null)
                {
                    Log.Warning("Task update failed. Column with ID {ColumnId} does not exist.", request.ColumnId);
                    return BadRequest($"Column with Id {request.ColumnId} does not exist.");
                }

                var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    Log.Warning("Task update failed. User with ID {UserId} does not exist.", request.UserId);
                    return BadRequest($"User with Id {request.UserId} does not exist.");
                }

                List<string> imageUrls = existingTask.ImageUrls ?? new List<string>();
                if (request.Files != null)
                {
                    foreach (var file in request.Files)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        using var stream = file.OpenReadStream();
                        var imageUrl = await _storageService.UploadImageAsync(stream, fileName);
                        imageUrls.Add(imageUrl);
                    }
                }

                existingTask.Name = request.Name;
                existingTask.Description = request.Description;
                existingTask.Status = request.Status;
                existingTask.Deadline = request.Deadline;
                existingTask.IsFavoriteTask = request.IsFavoriteTask;
                existingTask.ColumnId = request.ColumnId;
                existingTask.ImageUrls = imageUrls;
                existingTask.UserId = request.UserId;

                _unitOfWork.Tasks.Update(existingTask);
                await _unitOfWork.SaveChangesAsync();

                Log.Information("Task with ID {TaskId} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating task with ID {TaskId}.", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null)
                {
                    Log.Warning("Task deletion failed. Task with ID {TaskId} not found.", id);
                    return NotFound();
                }

                _unitOfWork.Tasks.Delete(task);
                await _unitOfWork.SaveChangesAsync();

                Log.Information("Task with ID {TaskId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting task with ID {TaskId}.", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
