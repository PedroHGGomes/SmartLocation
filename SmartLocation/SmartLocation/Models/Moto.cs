using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("MOTO")]
    public class Moto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_MOTO")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("MODELO")]
        [Display(Name = "MODELO")]
        public string Modelo { get; set; }

        [Required]
        [Column("ANO")]
        [Display(Name = "ANO")]
        public int Ano { get; set; }

        [StringLength(10)]
        [Column("PLACA")]
        [Display(Name = "PLACA")]
        public string? Placa { get; set; }
    }

}


