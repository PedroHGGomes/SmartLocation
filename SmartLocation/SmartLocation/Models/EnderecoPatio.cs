using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocation.Models
{
    [Table("ENDERECO_PATIO")]
    public class EnderecoPatio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_ENDERECO")]
        [Display(Name = "ID_ENDERECO")]
        public int Id { get; set; }

        [Required]
        [Column("LOGRADOURO")]
        [Display(Name = "LOGRADOURO")]
        public string Logradouro { get; set; }

        [Required]
        [Column("NUMERO")]
        [Display(Name = "NUMERO")]
        public string Numero { get; set; }

        [Required]
        [Column("ESTADO")]
        [Display(Name = "ESTADO")]
        public string Estado { get; set; }

        [Required]
        [Column("CEP")]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Required]
        [Column("NUMERO_PATIO")]
        [Display(Name = "NUMERO_PATIO")]
        public int Patio { get; set; } 
    }
}
