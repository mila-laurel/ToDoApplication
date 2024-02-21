using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ToDoListApplication.Service;
public class TelegramService
{
    private readonly ILogger<Notifications> _logger;
    private readonly TelegramBotClient _botClient;
    private readonly string _url;
    private readonly ChatId _chatId;

    public TelegramService(ILogger<Notifications> logger, IConfiguration configuration)
    {
        _logger = logger;
        var telegramBotToken = configuration["TelegramBotToken"];
        _chatId = configuration["TelegramChatId"];
        _url = configuration["Url"];
        _botClient = new TelegramBotClient(telegramBotToken);
    }

    public async Task<Message> SendAsync(string message, int? todoid = null)
    {
        try
        {
            if (_botClient != null)
            {
                InlineKeyboardMarkup inlineKeyboard = null;
                if (todoid != null)
                {
                    inlineKeyboard = new InlineKeyboardMarkup(new[]
                                     {
                                    new []
                                    {
                                        new InlineKeyboardButton("✔️ Mark Completed")
                                        {
                                            Url = $"{_url}ToDoEntries/MarkCompleted/{todoid}"
                                        }
                                    }
                                });
                }

                return await _botClient.SendTextMessageAsync(_chatId, message, replyMarkup: inlineKeyboard);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return null;
    }

}
