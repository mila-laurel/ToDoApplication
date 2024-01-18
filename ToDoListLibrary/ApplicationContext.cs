using Microsoft.EntityFrameworkCore;

namespace ToDoListLibrary
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<ToDoList> Lists { get; set; }

        public DbSet<ToDoEntry> Entities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=todolistdatabase;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoEntry>()
                .HasOne(p => p.Owner)
                .WithMany(b => b.EntryList)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
