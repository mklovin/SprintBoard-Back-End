using SprintBoard.DTOs;
using SprintBoard.Entities;
using System.Threading.Tasks;

namespace SprintBoard.Interfaces.IRepo
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetByActivationTokenAsync(string token);
    }
}
