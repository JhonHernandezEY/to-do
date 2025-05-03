using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DoubleV.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string RolId { get; set; }
        public required string Name { get; set; }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
