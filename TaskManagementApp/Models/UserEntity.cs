namespace TaskManagementApp.Models
{
    public class UserEntity
    {
        public int Id { get; set; }  // Primary Key
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }

        // Navigation property
        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}
