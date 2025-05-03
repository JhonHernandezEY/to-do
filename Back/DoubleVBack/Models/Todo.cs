using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DoubleV.Models
{
    public class Todo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string TodoId { get; set; }
        public required string Name { get; set; }
        public required string UsuarioId { get; set; }
        public required DateTime Deadline { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
