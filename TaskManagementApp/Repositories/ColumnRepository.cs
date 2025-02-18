using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Repositories
{
    public class ColumnRepository : GenericRepository<ColumnEntity>
    {
        public ColumnRepository(ApplicationDbContext context) : base(context) { }
    }
}
