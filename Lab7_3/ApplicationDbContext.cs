using Microsoft.EntityFrameworkCore;

namespace Lab7_3
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<MyEntity> MyEntities { get; set; } // Добавьте свои сущности
    }
}
