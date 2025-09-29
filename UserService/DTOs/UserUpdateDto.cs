namespace UserService.DTOs
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
}
