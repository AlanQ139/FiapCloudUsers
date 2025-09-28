using FiapCloudGamesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/Auth
    public class AuthController : ControllerBase
    {
        private readonly AuthService _userService;

        public AuthController(AuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var response = await _userService.RegisterAsync(request);
                
                return CreatedAtAction(nameof(Register), response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _userService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during login.");
            }
        }
    }
}

