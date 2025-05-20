using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("Sensor")]
    public class Sensor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Column("Numero")]
        [Display(Name = "NUMERO")]
        public int Numero { get; set; }
    }
}

