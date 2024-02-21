using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoListLibrary;

namespace ToDoListApplication.Service;
public class Notifications : BackgroundService
{
    private readonly ILogger<Notifications> _logger;
    private readonly TelegramService _telegramService;
    private readonly IServiceProvider _provider;
    private TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(1);
    private TimeSpan DueTime { get; set; } = TimeSpan.FromSeconds(0);
    private DateTime PlanDate { get; set; }
    protected DateTime NextPlanDate()
    {
        return PlanDate.Add(CheckInterval);
    }


    public Notifications(ILogger<Notifications> logger, IServiceProvider provider, TelegramService telegram)
    {
        _logger = logger;
        _provider = provider;
        _telegramService = telegram;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (DueTime > TimeSpan.Zero)
            this.PlanDate = DateTime.Now.Add(DueTime);
        else // иначе время исполнения прошло - формируем плановую дату с учетом DueTime
            this.PlanDate = DateTime.Now.Add(DueTime).Add(CheckInterval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;

                if (PlanDate <= currentDateTime)
                {
                    using var scope = _provider.CreateScope();
                    var context = scope.ServiceProvider.GetService<ApplicationContext>();
                    string message = string.Empty;
                    var today = await context.Entities
                   .Where(l => l.Completed != Status.Completed && l.DueDate.Date == DateTime.Today)
                   .ToListAsync();
                    if (today.Count > 0)
                    {
                        message = $"Check your today tasks - you have {today.Count} tasks which due date is today";
                        await _telegramService.SendAsync(message);
                        foreach (var item in today)
                        {
                            await _telegramService.SendAsync(item.Title, item.Id);
                        }
                    }
                }
                else
                {
                    var delayCorrection = PlanDate - currentDateTime;
                    await Task.Delay(delayCorrection, stoppingToken);
                    continue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }
}
