
using Makman.Middle.Entities.Connections;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls.Primitives;

namespace Makman.Middle.Entities.DatabaseContexts
{
    internal class LocalDatabaseContext : DbContext
    {
        readonly private string _filename;
        public LocalDatabaseContext(string filename)
        {
            _filename = filename;
        }
        public DbSet<CollectionDirectory> CollectionDirectories { get; set; } = null!;
        public DbSet<TagCategory> TagCategories { get; set; } = null!;
        public DbSet<Bunch> Bunchs { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + _filename);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Units)
                .UsingEntity<UnitTag>();

            modelBuilder.Entity<Unit>()
                .HasOne(e => e.Bunch)
                .WithMany(e => e.Units)
                .HasForeignKey(e => e.BunchId)
                .IsRequired(false);

            modelBuilder.Entity<Unit>()
                .HasOne(e => e.ParentUnit)
                .WithMany(e => e.ChildUnits)
                .HasForeignKey(e => e.ParentUnitId)
                .IsRequired(false);

            modelBuilder.Entity<Tag>()
                .HasOne(e => e.Category)
                .WithMany()
                .IsRequired(false);

            modelBuilder.Entity<Tag>()
                .HasOne(e => e.ParentTag)
                .WithOne()
            .IsRequired(false);

            modelBuilder.Entity<Unit>().Navigation(e => e.Tags).AutoInclude();
            modelBuilder.Entity<Tag>().Navigation(e => e.Category).AutoInclude();
            modelBuilder.Entity<Bunch>().Navigation(e => e.Units).AutoInclude();

        }
    }
}
