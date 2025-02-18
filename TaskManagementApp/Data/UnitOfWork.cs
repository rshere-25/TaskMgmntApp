using System.Threading.Tasks;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Data
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public TaskRepository Tasks { get; }
        public ColumnRepository Columns { get; }

        public UserRepository Users { get; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Tasks = new TaskRepository(_context);
            Columns = new ColumnRepository(_context);
            Users = new UserRepository(_context);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
