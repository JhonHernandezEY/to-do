using DoubleV.Models;

namespace DoubleV.DTOs
{
    public class UserWithRoleDTO
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public int? RolId { get; set; }
        public string? RolName { get; set; }
    }
}
