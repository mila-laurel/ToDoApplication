using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoListLibrary
{
    public class Repository : IRepository
    {
        private readonly ApplicationContext context;

        public Repository(ApplicationContext context)
        {
            this.context = context;
        }

        public ToDoList CreateList(string title)
        {
            var list = new ToDoList(title);
            list.EntryList = new List<ToDoEntry>();
            context.Lists.Add(new ToDoList(title));
            context.SaveChanges();
            return list;
        }

        public ToDoEntry CreateToDoEntry(string title, ToDoList list)
        {
            var entry = new ToDoEntry(title);
            context.Entities.Add(entry);
            entry.Owner = list;
            entry.CreatedOn = DateTime.Now;
            context.SaveChanges();
            return entry;
        }

        public void DeleteList(ToDoList list)
        {
            context.Lists.Remove(list);
            context.SaveChanges();
        }

        public void HideList(ToDoList list)
        {
            list.Hide = true;
            context.SaveChanges();
        }

        public void ModifyToDoList(ToDoList list, ToDoList newData)
        {
            list.Title = newData.Title;
            list.Hide = newData.Hide;
            context.SaveChanges();
        }

        public void ModifyToDoEntry(ToDoEntry entry, ToDoEntry newData)
        {
            entry.Title = newData.Title;
            entry.DueDate = newData.DueDate;
            entry.Description = newData.Description;
            entry.Completed = newData.Completed;
            context.SaveChanges();
        }

        public List<ToDoList> GetAllLists(bool includeHidden = false)
        {
            var lists = context.Lists
                .Where(l => includeHidden || !l.Hide)
                .Include(l => l.EntryList)
                .ToList();
            return lists;
        }

        public void DeleteEntry(ToDoEntry record)
        {
            context.Entities.Remove(record);
            context.SaveChanges();
        }
    }
}
