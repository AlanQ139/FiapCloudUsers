using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Controllers;
using UserService.DTOs;
using UserService.Models;
using Xunit;

namespace UserService.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _controller = new UsersController(_repoMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithUserResponseDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@example.com", Role = "User", CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Role = "Admin", CreatedAt = DateTime.UtcNow }
            };
            _repoMock.Setup(r => r.GetAllAsync(1, 50)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll(1, 50);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);
            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async Task GetById_UserExists_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "Alice", Email = "alice@example.com", Role = "User", CreatedAt = DateTime.UtcNow };
            _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(userId, dto.Id);
        }

        [Fact]
        public async Task GetById_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_UserExists_UpdatesAndReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "Alice", Role = "User", IsActive = true };
            var dto = new UserUpdateDto { Name = "Alice Updated", Role = "Admin", IsActive = false };
            _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.Update(userId, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Name == dto.Name && u.Role == dto.Role && u.IsActive == dto.IsActive)), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new UserUpdateDto { Name = "Test" };
            _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Update(userId, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_UserExists_DeletesAndReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _repoMock.Verify(r => r.DeleteAsync(user), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
