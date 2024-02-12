using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using ToDoListLibrary;

namespace ToDoAspNetMvc.ViewModels;

public class ToDoEntryViewModel
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    [DisplayName("Due date")]
    public DateTime DueDate { get; set; }

    [DisplayName("Status")]
    public Status Completed { get; set; } = Status.NotStarted;

    public List<SelectListItem> ToDoLists { set; get; }

    public List<CustomFieldViewModel> Fields { get; set; } = new List<CustomFieldViewModel>();
}
