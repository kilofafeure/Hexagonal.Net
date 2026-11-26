using Microsoft.EntityFrameworkCore;
using Repository.Model;

namespace Repository
{
    public class ItemsDbContext(DbContextOptions<ItemsDbContext> options) : DbContext(options)
    {
        public DbSet<ItemModel> ItemsModel { get; set; } = null!;
        public DbSet<NoteModel> NotesModel { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemModel>().ToTable("Item");
            modelBuilder.Entity<NoteModel>().ToTable("Note")
                .HasOne(n => n.Item)
                .WithMany(n => n.Notes)
                .HasForeignKey(n => n.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
