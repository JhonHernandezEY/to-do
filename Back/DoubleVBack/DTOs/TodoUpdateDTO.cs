namespace DoubleV.DTOs
{
    public class UpdatedTodoDTO
    {
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public required DateTime Deadline { get; set; }
    }
}
