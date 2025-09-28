using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/Users
    [Authorize] // Todos os endpoints aqui requerem autenticação por padrão
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Apenas admins podem listar todos os usuários
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || (new Guid(userIdClaim) != id && !User.IsInRole(Role.UserRole.Admin.ToString())))
            {
                return Forbid("You do not have permission to view this profile.");
            }

            var user = await _userService.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || (new Guid(userIdClaim) != id && !User.IsInRole(Role.UserRole.Admin.ToString())))
            {
                return Forbid("You do not have permission to update this profile.");
            }

            var updatedUser = await _userService.UpdateUserProfileAsync(id, request);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Apenas admins podem deletar usuários
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content para deleções bem-sucedidas
        }
    }
}
