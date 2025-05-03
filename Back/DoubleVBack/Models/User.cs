using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Text.Json.Serialization;

namespace DoubleV.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? RolId { get; set; }

        [JsonIgnore]
        public Role? Rol { get; set; }

        [JsonIgnore]
        public ICollection<Todo>? Todos { get; set; }

        public override string ToString()
        {
            return $"{Name} ({UserId})"; 
        }
    }
}
