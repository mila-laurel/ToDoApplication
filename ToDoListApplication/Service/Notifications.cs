using Microsoft.EntityFrameworkCore;
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
    private readonly ApplicationContext _context;
    private TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(10);
    private TimeSpan DueTime { get; set; } = TimeSpan.FromSeconds(0);
    private DateTime PlanDate { get; set; }
    protected DateTime NextPlanDate()
    {
        return PlanDate.Add(CheckInterval);
    }


    public Notifications(ILogger<Notifications> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
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
                    string message = string.Empty;
                    var today = await _context.Entities
                   .Where(l => l.Completed != Status.Completed && l.DueDate.Date == DateTime.Today)
                   .CountAsync();
                    if (today > 0)
                        message = $"Check your today tasks - you have {today} tasks which due date is today";
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
                _logger.LogError(ex, null);
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }
}
