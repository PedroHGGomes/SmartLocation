using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("SENSOR")]
    public class Sensor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_SENSOR")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Column("NUMERO")]
        [Display(Name = "NUMERO")]
        public int Numero { get; set; }
    }
}

