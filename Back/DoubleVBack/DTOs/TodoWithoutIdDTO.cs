namespace DoubleV.DTOs
{
    public class TodoWithoutIdDTO
    {        
        public required string Name { get; set; }
        public required int UserId { get; set; }
        public required DateTime DeadLine { get; set; }
    }
}
