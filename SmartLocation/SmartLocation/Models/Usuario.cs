using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_USUARIO")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NOME")]
        [Display(Name = "NOME")]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Column("EMAIL")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
    }
}

