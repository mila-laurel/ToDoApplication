using Microsoft.EntityFrameworkCore;
using ToDoListLibrary;

namespace ToDoListApplication.Tests;

internal class TestApplicationContext : ApplicationContext
{
    public TestApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
