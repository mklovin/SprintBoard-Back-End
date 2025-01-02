using Microsoft.AspNetCore.Mvc;
using SprintBoard.DTOs;
using SprintBoard.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
     
        [HttpPost("signup")]
        public async Task<ActionResult<Response<UserDto>>> SignUp([FromBody] UserDto userDto)
        {
            var response = await _userService.CreateUserAsync(userDto);

            if (!response.IsSuccess)
            {
                return BadRequest(response); 
            }

            return Ok(response); 
        }


        [HttpGet("activate")]
        public async Task<IActionResult> ActivateAccount([FromQuery] string token)
        {
            var result = await _userService.ActivateAccountAsync(token);
            return result ? Ok("Account activated") : BadRequest("Invalid or expired token");
        }

        [HttpPost("signin")]
        public async Task<ActionResult<UserDto>> SignIn([FromBody] SignInDto signInDto)
        {
            var user = await _userService.AuthenticateUserAsync(signInDto.Email, signInDto.Password);
            if (user == null)
            {
                return Unauthorized("StatusCode:401");
            }

            return Ok(user);
        }

    }
}

