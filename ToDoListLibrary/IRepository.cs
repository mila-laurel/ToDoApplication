using System.Collections.Generic;

namespace ToDoListLibrary;

public interface IRepository
{
    ToDoList CreateList(string title);
    ToDoEntry CreateToDoEntry(string title, ToDoList list);
    void DeleteEntry(ToDoEntry record);
    void DeleteList(ToDoList list);
    List<ToDoList> GetAllLists(bool includeHidden = false);
    void HideList(ToDoList list);
    void ModifyToDoEntry(ToDoEntry entry, ToDoEntry newData);
    void ModifyToDoList(ToDoList list, ToDoList newData);
}