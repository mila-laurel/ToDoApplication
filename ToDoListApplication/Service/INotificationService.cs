using System.Threading.Tasks;

namespace ToDoListApplication.Service;

public interface INotificationService
{
    Task SendNotification();
}
