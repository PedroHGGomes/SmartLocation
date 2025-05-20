using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Nome")]
        [Display(Name = "NOME")]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Column("Email")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
    }
}

