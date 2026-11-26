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
            modelBuilder.Entity<ItemModel>()
                .ToTable("Item");

            modelBuilder.Entity<NoteModel>(entity =>
            {
                entity.ToTable("Note");
                entity.Property(note => note.Color)
                    .HasColumnType("varchar(6)")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.HasOne(note => note.Item)
                    .WithMany(item => item.Notes)
                    .HasForeignKey(note => note.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
