using DoubleV.Models;

namespace DoubleV.DTOs
{
    public class UserResponse
    {
        public required string Message { get; set; }
        public required List<User> Users { get; set; }
    }
}
