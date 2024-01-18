using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoListLibrary
{
    public class ToDoEntry
    {
        public ToDoEntry(string title) 
        { 
            this.Title = title;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ToDoList Owner { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public Status Completed { get; set; } = Status.X;
    }

    public enum Status 
    { 
        V,
        X
    }
}
