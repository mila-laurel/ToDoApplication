using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApplication.Service;
public class TelegramService
{
    public TelegramService(IConfiguration configuration)
    {
        var telegramBotToken = configuration["TelegramBotToken"];
        var telegramChatId = configuration["TelegramChatId"];

    }
}
