using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo) => _repo = repo;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var users = await _repo.GetAllAsync(page, pageSize);
            var result = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            var dto = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
            return Ok(dto);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Name = dto.Name ?? user.Name;
            user.Role = dto.Role ?? user.Role;
            if (dto.IsActive.HasValue) user.IsActive = dto.IsActive.Value;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(user);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            await _repo.DeleteAsync(user);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
    }

}
