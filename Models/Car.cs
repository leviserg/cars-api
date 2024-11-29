using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cars_api.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        [Required]
        [MaxLength(100)]
        [Column(TypeName ="nvarchar(100)")]
        public required string Name { get; set; }

        [Required]
        [Range(1800,9999)]
        public required int Year { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastChangedAtUtc { get; set; } = DateTime.UtcNow;
        public int BrandId { get; set; }

        public Brand? Brand { get; set; }
    }
}
