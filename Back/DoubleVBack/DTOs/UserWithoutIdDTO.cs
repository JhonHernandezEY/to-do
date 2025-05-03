namespace DoubleV.DTOs
{
    public class UserWithoutIdDTO
    {        
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int? RolId { get; set; }
    }
}
