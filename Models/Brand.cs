using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cars_api.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public required string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastChangedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
