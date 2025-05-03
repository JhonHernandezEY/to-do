namespace DoubleV.DTOs
{
    public class TodoWithUserDTO
    {
        public required string TodoId { get; set; }
        public required string Name { get; set; }        
        public required DateTime Deadline { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
    }
}
