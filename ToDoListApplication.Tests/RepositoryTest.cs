using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoListLibrary;

#pragma warning disable CA1707
#pragma warning disable SA1600
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1001 // Types that own disposable fields should be disposable

namespace ToDoListApplication.Tests;

[TestFixture]
public class RepositoryTest
{
    private DbContextOptions<ApplicationContext> dbContextOptions;

    private ApplicationContext context;

    [SetUp]
    public void Init()
    {
        var dbName = $"UserDb_{DateTime.Now.ToFileTimeUtc()}";
        this.dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(dbName).Options;
        this.context = new TestApplicationContext(this.dbContextOptions);
        this.PopulateData(this.context);
    }

    [TearDown]
    public void Cleanup()
    {
        this.context.Dispose();
        this.context = null;
    }

    [Test]
    public void TestGetAllLists_ReturnsRows()
    {
        var repository = new Repository(this.context);

        var result = repository.GetAllLists();

        Assert.AreEqual(2, result.Count);
    }

    [Test]
    public void TestCreateList_Success()
    {
        var repository = new Repository(this.context);

        repository.CreateList("New List");

        Assert.AreEqual(3, repository.GetAllLists().Count);
    }

    [Test]
    public void TestCreateToDoEntry_Success()
    {
        const string newEntryTitle = "New To Do";
        var repository = new Repository(this.context);
        var list = repository.GetAllLists().First();

        repository.CreateToDoEntry(newEntryTitle, list);

        var updatedList = repository.GetAllLists().First();

        Assert.AreEqual(2, updatedList.EntryList.Count);
        Assert.True(updatedList.EntryList.Exists(e => e.Title == newEntryTitle));
    }

    [Test]
    public void TestModifyList_Success()
    {
        const string newListTitle = "New Title";
        var repository = new Repository(this.context);
        var list = repository.GetAllLists().First();

        repository.ModifyToDoList(list, new ToDoList(newListTitle));

        var updatedList = repository.GetAllLists().First();

        Assert.AreEqual(newListTitle, updatedList.Title);
    }

    [Test]
    public void TestHideList_Success()
    {
        var repository = new Repository(this.context);
        var list = repository.GetAllLists().First();

        repository.HideList(list);

        Assert.AreEqual(1, repository.GetAllLists().Count);
        Assert.AreEqual(2, repository.GetAllLists(true).Count);
    }

    [Test]
    public void TestDeleteList_Success()
    {
        var repository = new Repository(this.context);
        var list = repository.GetAllLists().First();

        repository.DeleteList(list);

        Assert.AreEqual(1, repository.GetAllLists().Count);
        Assert.AreEqual(1, repository.GetAllLists(true).Count);
    }

    [Test]
    public void TestModifyEntry_Success()
    {
        const string description = "full description of to do entry";
        var repository = new Repository(this.context);
        var entry = repository.GetAllLists().First().EntryList.First();

        repository.ModifyToDoEntry(entry, new ToDoEntry(entry.Title)
        {
            Description = description,
        });

        var updatedEntry = repository.GetAllLists().First().EntryList.First();

        Assert.AreEqual(description, updatedEntry.Description);
    }

    [Test]
    public void TestDeleteEntry_Success()
    {
        var repository = new Repository(this.context);
        var entry = repository.GetAllLists().First().EntryList.First();

        repository.DeleteEntry(entry);

        Assert.AreEqual(0, repository.GetAllLists().First().EntryList.Count);
    }

    private void PopulateData(ApplicationContext context)
    {
        context.Lists.Add(new ToDoList("Some chores") { EntryList = new List<ToDoEntry>() { new ToDoEntry("Wash clothes") } });
        context.Lists.Add(new ToDoList("Some work stuff"));
        context.SaveChanges();
    }
}
