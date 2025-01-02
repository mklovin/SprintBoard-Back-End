using Microsoft.EntityFrameworkCore;
using SprintBoard.Entities;
using SprintBoard.Interfaces.IRepo;
using System.Threading.Tasks;

namespace SprintBoard.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByActivationTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ActivationToken == token && u.ActivationTokenExpiry > DateTime.UtcNow);
        }
    }
}
