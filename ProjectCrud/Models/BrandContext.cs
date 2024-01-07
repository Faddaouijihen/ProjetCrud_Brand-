using Microsoft.EntityFrameworkCore;

namespace ProjectCrud.Models
{
    public class BrandContext : DbContext
    {
        // Propriété Brands initialisée à une valeur non nulle (une instance DbSet vide).
        public DbSet<Brand> Brands { get; set; } = null!;

        public BrandContext(DbContextOptions<BrandContext> options) : base(options)
        {
            // Initialise la propriété Brands avec une instance DbSet vide.
            Brands = Set<Brand>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurer la propriété ID pour autoriser l'insertion explicite (si nécessaire)
            modelBuilder.Entity<Brand>()
                .Property(b => b.id)
                .ValueGeneratedNever(); // ou .ValueGeneratedOnAdd(), selon vos besoins
        }
    }
}
