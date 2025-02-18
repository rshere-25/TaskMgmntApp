
namespace TaskManagementApp.DTOs
{
    public class TaskCreateRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? Deadline { get; set; }
        public bool IsFavoriteTask { get; set; }
        public string? Status { get; set; }
        public int ColumnId { get; set; }
        public IFormFile[]? Files { get; set; }

        public int UserId { get; set; }

        public int Id { get; set; }

    }
}
