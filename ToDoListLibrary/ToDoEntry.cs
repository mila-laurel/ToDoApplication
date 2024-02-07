using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;

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

        [DisplayName("Due date")]
        public DateTime DueDate { get; set; }

        [DisplayName("Status")]
        public Status Completed { get; set; } = Status.NotStarted;

        public DateTime CreatedOn { get; set; }

        public List<CustomField> Fields { get; set; } = new List<CustomField>();
    }

    public enum Status 
    {
        NotStarted,
        InProgress,
        Completed
    }
}
