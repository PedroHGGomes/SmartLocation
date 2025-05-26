using Microsoft.EntityFrameworkCore;

namespace SmartLocation.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<EnderecoPatio> EnderecosPatio { get; set; }

        public DbSet<Moto> Moto { get; set; }

        public DbSet<Sensor> Sensor { get; set; }
    }
}


