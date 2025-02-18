using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateOnly? Deadline { get; set; }

        public bool IsFavoriteTask { get; set; }

        [Required]
        public string? Status { get; set; }

        // Foreign Key to UserEntity
        public int UserId { get; set; }

        // Navigation property
        public UserEntity? User { get; set; }

        [ForeignKey("ColumnId")]
        public int ColumnId { get; set; }
        public ColumnEntity? Column { get; set; }

        public List<string>? ImageUrls { get; set; }
    }

    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
}
