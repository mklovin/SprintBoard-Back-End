using SprintBoard.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintBoard.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<Response<UserDto>> CreateUserAsync(UserDto userDto);
        Task<bool> UpdateUserAsync(Guid userId, UserDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> ActivateAccountAsync(string token);
        Task<UserDto> SignUpAsync(UserDto userDto);
        Task<UserDto> AuthenticateUserAsync(string email, string password);


    }
}
