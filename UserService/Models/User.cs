using static UserService.Models.Role;

namespace UserService.Models
{
    //public enum UserRole { User, Admin }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        //TODO entender como relacionar com o projeto de Games no microservices
        //no Monolito estava como list<game>
    }
}
