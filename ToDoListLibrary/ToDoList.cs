using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoListLibrary;

public class ToDoList
{
    public ToDoList()
    {
        
    }

    public ToDoList(string title)
    {
        Title = title;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; }

    public bool Hide { get; set; }

    public List<ToDoEntry> EntryList { get; set; } = new List<ToDoEntry>();
}
