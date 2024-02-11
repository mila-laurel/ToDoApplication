using Microsoft.EntityFrameworkCore;
using ToDoListLibrary;

#pragma warning disable SA1600
#pragma warning disable SA1502 // Element should not be on a single line

namespace ToDoListApplication.Tests;

internal class TestApplicationContext : ApplicationContext
{
    public TestApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
