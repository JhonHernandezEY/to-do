using DoubleV.Models;

namespace DoubleV.DTOs
{
    public class UsersWithRoleResponse
    {
        public required string Message { get; set; }
        public required List<UserWithRoleDTO> Users { get; set; }
    }
}
