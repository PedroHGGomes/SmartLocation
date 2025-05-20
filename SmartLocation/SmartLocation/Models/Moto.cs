using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("Moto")]
    public class Moto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Modelo")]
        [Display(Name = "MODELO")]
        public string Modelo { get; set; }

        [Required]
        [Column("Ano")]
        [Display(Name = "ANO")]
        public int Ano { get; set; }

        [StringLength(10)]
        [Column("Placa")]
        [Display(Name = "PLACA")]
        public string? Placa { get; set; }
    }

}


