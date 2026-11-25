using Microsoft.EntityFrameworkCore;
using Repository.Model;

namespace Repository
{
    public class ItemsDbContext(DbContextOptions<ItemsDbContext> options) : DbContext(options)
    {
        public DbSet<ItemModel> ItemsModel { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemModel>().ToTable("Item");
        }
    }
}
