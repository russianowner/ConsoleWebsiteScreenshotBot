#nullable disable
using ScreenshotBot.Services;

namespace ScreenshotBot
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Введите токен бота:");
            string token = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Токен не введен");
                return;
            }
            var telegramService = new TelegramService(token);
            await telegramService.StartAsync();
        }
    }
}
