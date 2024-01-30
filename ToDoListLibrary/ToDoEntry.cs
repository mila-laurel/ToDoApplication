using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ToDoListLibrary
{
    public class ToDoEntry
    {
        public ToDoEntry()
        {
            
        }

        public ToDoEntry(string title) 
        { 
            this.Title = title;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        public virtual ToDoList Owner { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public Status Completed { get; set; } = Status.NotStarted;

        public DateTime CreatedOn { get; set; }

        public List<CustomField> Fields { get; set; }
    }

    public enum Status 
    {
        NotStarted,
        InProgress,
        Completed
    }
}
