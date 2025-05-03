using DoubleV.Models;

namespace DoubleV.DTOs
{
    public class RolesResponse
    {
        public required string Message { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
