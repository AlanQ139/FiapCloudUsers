using FiapCloudGamesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;
using MassTransit;
using Shared.Contracts;

namespace UserService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IAuthService _auth;
        private readonly IPublishEndpoint _publishEndpoint; //adicionado para publicar eventos
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository repo, IAuthService auth, IPublishEndpoint publishEndpoint, ILogger<AuthController> logger)
        {
            _repo = repo;
            _auth = auth;
            _publishEndpoint = publishEndpoint; // para publicar eventos
            _logger = logger;
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

            // Publicar evento UserRegistered
            // novo usuario registrado
            try
            {
                await _publishEndpoint.Publish<IUserRegistered>(new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    RegisteredAt = user.CreatedAt
                });

                _logger.LogInformation(
                    "Usuário registrado e evento publicado: User={UserId}, Email={Email}",
                    user.Id, user.Email);
            }
            catch (Exception ex)
            {
                // Log mas não falha o registro (evento é "nice to have")
                _logger.LogWarning(ex, "Falha ao publicar evento UserRegistered");
            }

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
            if (user == null)
                return Unauthorized("Invalid credentials.");

            if (!_auth.VerifyPassword(user.PasswordHash, dto.Password))
                return Unauthorized("Invalid credentials.");

            var token = _auth.GenerateToken(user);

            _logger.LogInformation("Login bem-sucedido: User={UserId}", user.Id);

            return Ok(new { token });
        }
    }
}

