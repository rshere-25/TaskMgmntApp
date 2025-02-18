using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

    }
}
