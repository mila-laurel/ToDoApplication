namespace ToDoAspNetMvc.Models;

public class MarkCompletedViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsCompleted { get; set; }
}
