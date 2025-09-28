using UserService.Models;

namespace UserService.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetByEmailAsync(string email);
    }
}
