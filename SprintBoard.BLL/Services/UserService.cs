using AutoMapper;
using SprintBoard.DTOs;
using SprintBoard.Entities;
using SprintBoard.Interfaces;
using SprintBoard.Interfaces.IRepo;
using System.Text;
using System.Security.Cryptography;
using SprintBoard.BLL.Extensions;

namespace SprintBoard.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;   
            _mapper = mapper;
            _emailService = emailService;

        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<Response<UserDto>> CreateUserAsync(UserDto userDto)
        {
            // Check if email is valid
            if (string.IsNullOrWhiteSpace(userDto.Email) || !userDto.Email.IsValidEmail())
            {
                return Response<UserDto>.Failure("Invalid email format.");
            }

            // Check if email is already used
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return Response<UserDto>.Failure("Email is already in use.");
            }

            // Validate the user object
            if (!userDto.IsValid(out string validationError))
            {
                return Response<UserDto>.Failure(validationError);
            }

            // Validate password complexity
            if (!userDto.Password.IsPasswordComplex())
            {
                return Response<UserDto>.Failure("Password does not meet complexity requirements.");
            }

            // Map and hash the password
            var user = _mapper.Map<User>(userDto);
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = HashPassword(userDto.Password);
            }

            // Add user to the repository
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var createdUserDto = _mapper.Map<UserDto>(user);
            return Response<UserDto>.Success(createdUserDto, "User created successfully.");
        }


        public async Task<bool> UpdateUserAsync(Guid userId, UserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);
            if (existingUser == null) return false;

            _mapper.Map(updateUserDto, existingUser);
            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<UserDto> SignUpAsync(UserDto userDto)
        {
            if (await _userRepository.GetUserByEmailAsync(userDto.Email) != null)
                throw new ArgumentException("Email already in use.");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = HashPassword(userDto.Password),
                ActivationToken = GenerateActivationToken(),
                ActivationTokenExpiry = DateTime.UtcNow.AddHours(24),
                IsActivated = false
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            await SendActivationEmail(user);

            return userDto;
        }

        private async System.Threading.Tasks.Task SendActivationEmail(User user)
        {
            var activationLink = $"https://localhost:5001/api/users/activate?token={user.ActivationToken}";
            var subject = "Activate your account";
            var body = $"Hello {user.Username},\n\nPlease activate your account by clicking the link below:\n{activationLink}\n\nThank you!";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private string GenerateActivationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<bool> ActivateAccountAsync(string token)
        {
            var user = await _userRepository.GetByActivationTokenAsync(token);
            if (user == null || user.ActivationTokenExpiry < DateTime.UtcNow)
                return false;

            user.IsActivated = true;
            user.ActivationToken = null;
            user.ActivationTokenExpiry = null;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return _mapper.Map<UserDto>(user);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using var sha256 = SHA256.Create();
            var hashedInput = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hashedInput == hashedPassword;
        }


    }
}
