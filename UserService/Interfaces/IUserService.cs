using UserService.DTOs;

namespace UserService.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetUserProfileAsync(Guid id);
        Task<UserProfileResponse> UpdateUserProfileAsync(Guid id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserProfileResponse>> GetAllUsersAsync(); // Para admins
    }
}
