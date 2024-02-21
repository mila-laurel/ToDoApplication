using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ToDoListLibrary;

namespace ToDoListApplication.Tests;

[TestFixture]
public class Tests
{
    private DbContextOptions<ApplicationContext> _dbContextOptions;
    private ApplicationContext _context;

    [SetUp]
    public void Init()
    {
        var dbName = $"UserDb_{DateTime.Now.ToFileTimeUtc()}";
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(dbName).Options;
        _context = new TestApplicationContext(_dbContextOptions);
        PopulateData(_context);
    }

    [TearDown]
    public void Cleanup()
    {

    }

    [Test]
    public void TestGetAllLists_ReturnsRows()
    {

    }

    private void PopulateData(ApplicationContext context)
    {
        context.Lists.Add(new ToDoList("Some chores") { EntryList = new List<ToDoEntry>() { new ToDoEntry("Wash clothes") } });
        context.Lists.Add(new ToDoList("Some work stuff"));
        context.SaveChanges();
    }
}
