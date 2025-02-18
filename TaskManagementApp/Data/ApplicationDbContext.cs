using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;

namespace TaskManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ColumnEntity> Columns { get; set; }
        public DbSet<UserEntity> Users { get; set; } // Add DbSet for UserEntity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Column data
            modelBuilder.Entity<ColumnEntity>()
                .HasData(
                    new ColumnEntity { Id = 1, Name = "ToDo" },
                    new ColumnEntity { Id = 2, Name = "InProgress" },
                    new ColumnEntity { Id = 3, Name = "Done" }
                );

            // Task-Column Relationship
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.Column)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task-User Relationship: Use DeleteBehavior.Cascade (if you want to delete tasks with users)
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.User)  // Task has a relationship with User
                .WithMany(u => u.Tasks)  // User can have multiple tasks
                .HasForeignKey(t => t.UserId)  // Foreign key for User in Task
                .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, delete the associated tasks

            base.OnModelCreating(modelBuilder);
        }
    }
}
