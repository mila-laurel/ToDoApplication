using Microsoft.EntityFrameworkCore;

namespace ToDoListLibrary;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<ToDoList> Lists { get; set; }

    public DbSet<ToDoEntry> Entities { get; set; }

    public DbSet<CustomField> CustomFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoEntry>()
            .HasOne(p => p.Owner)
            .WithMany(b => b.EntryList)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomField>()
            .HasOne<ToDoEntry>()
            .WithMany(entry => entry.Fields);
    }
}
