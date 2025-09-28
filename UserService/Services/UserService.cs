using System.Security.Claims;
using System.Text;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

//precisa refatorar agora que é um microsserviço
//revisar o que precisa do monolito FIAPCloudGamesProject
namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration; // Para JWT

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        //esta com erro para arrumar, o authresponse n existe
        //public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request)
        //{
        //    var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        //    if (existingUser != null)
        //    {
        //        throw new InvalidOperationException("User with this email already exists.");
        //    }

        //    var user = new User
        //    {
        //        //Role = Role.UserRole.User,
        //        //Nome = request.Nome,
        //        //Email = request.Email,
        //        //SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
        //        //DataCadastro = DateTime.UtcNow
        //        Role = Role.UserRole.User,
        //        Name = request.Name,
        //        Email = request.Email,
        //        CreatedAt = DateTime.UtcNow,
        //        Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        //    };

        //    await _userRepository.AddUserAsync(user);

        //    // Gerar token após registro
        //    return GenerateJwtToken(user);
        //}
        //esta com erro para arrumar
        //public async Task<AuthResponse> LoginAsync(LoginRequest request)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(request.Email);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        //    {
        //        throw new UnauthorizedAccessException("Invalid credentials.");
        //    }

        //    return GenerateJwtToken(user);
        //}
        //esta com erro para arrumar
        //private AuthResponse GenerateJwtToken(User user)
        //{
        //    var jwtKey = _configuration["Jwt:Key"];
        //    var jwtIssuer = _configuration["Jwt:Issuer"];
        //    var jwtAudience = _configuration["Jwt:Audience"];

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //        new Claim(ClaimTypes.Name, user.Nome),
        //        new Claim(ClaimTypes.Role, user.Role.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    var token = new JwtSecurityToken(
        //        issuer: jwtIssuer,
        //        audience: jwtAudience,
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(60), // Token expira em 60 minutos
        //        signingCredentials: credentials);

        //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //    return new AuthResponse
        //    {
        //        Token = tokenString,
        //        Expiration = token.ValidTo,
        //        User = new UserProfileResponse
        //        {
        //            Id = user.Id,
        //            Nome = user.Nome,
        //            Email = user.Email,
        //            Role = user.Role,
        //            DataCadastro = user.DataCadastro
        //        }
        //    };
        //}
        //esta com erro para arrumar
        //public async Task<UserProfileResponse> GetUserProfileAsync(Guid id)
        //{
        //    var user = await _userRepository.GetUserByIdAsync(id);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    return new UserProfileResponse
        //    {
        //        Id = user.Id,
        //        Nome = user.Nome,
        //        Email = user.Email,
        //        Role = user.Role,
        //        DataCadastro = user.DataCadastro
        //    };
        //}
        //esta com erro para arrumar
        //public async Task<UserProfileResponse> UpdateUserProfileAsync(Guid id, UpdateUserRequest request)
        //{
        //    var user = await _userRepository.GetUserByIdAsync(id);
        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    user.Nome = request.Nome;
        //    user.Email = request.Email;
        //    // Não atualiza senha aqui.

        //    await _userRepository.UpdateUserAsync(user);

        //    return new UserProfileResponse
        //    {
        //        Id = user.Id,
        //        Nome = user.Nome,
        //        Email = user.Email,
        //        Role = user.Role,
        //        DataCadastro = user.DataCadastro
        //    };
        //}

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            await _userRepository.DeleteUserAsync(id);
            return true;
        }
        //Adicionados sem implemnetação para compilar, restante do codigo esta com erro
        //não entendi o erro ainda
        public Task<IEnumerable<UserProfileResponse>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileResponse> GetUserProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileResponse> UpdateUserProfileAsync(Guid id, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
        //esta com erro para arrumar
        //public async Task<IEnumerable<UserProfileResponse>> GetAllUsersAsync()
        //{
        //    var users = await _userRepository.GetAllUsersAsync();
        //    return users.Select(user => new UserProfileResponse
        //    {
        //        Id = user.Id,
        //        Nome = user.Nome,
        //        Email = user.Email,
        //        Role = user.Role,
        //        DataCadastro = user.DataCadastro
        //    });
        //}
    }
}
