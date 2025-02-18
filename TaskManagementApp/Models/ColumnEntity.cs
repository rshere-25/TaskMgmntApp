using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class ColumnEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}
