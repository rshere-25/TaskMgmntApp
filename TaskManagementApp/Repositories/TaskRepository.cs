using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Repositories
{
    public class TaskRepository : GenericRepository<TaskEntity>
    {
        public TaskRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<TaskEntity>> GetAllWithColumnsAsync()
        {
            return await _context.Tasks.Include(t => t.Column).ToListAsync();
        }
    }
}
