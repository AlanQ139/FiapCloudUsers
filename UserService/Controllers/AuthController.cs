using FiapCloudGamesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IAuthService _auth;

        public AuthController(IUserRepository repo, IAuthService auth)
        {
            _repo = repo;
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            var exists = await _repo.GetByEmailAsync(dto.Email);
            if (exists != null) return Conflict("Email already in use.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _auth.HashPassword(dto.Password),
                Role = dto.Role ?? "User"
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            var response = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return CreatedAtAction(nameof(Register), response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Invalid credentials.");

            if (!_auth.VerifyPassword(user.PasswordHash, dto.Password)) return Unauthorized("Invalid credentials.");

            var token = _auth.GenerateToken(user);
            return Ok(new { token });
        }
    }

}

