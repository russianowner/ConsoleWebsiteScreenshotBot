using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScreenshotBot.Services
{
    public class TelegramService
    {
        private readonly TelegramBotClient _botClient;
        private readonly ScreenshotService _screenshotService;

        public TelegramService(string token)
        {
            _botClient = new TelegramBotClient(token);
            _screenshotService = new ScreenshotService();
        }
        public async Task StartAsync()
        {
            using var cts = new System.Threading.CancellationTokenSource();
            var receiverOptions = new Telegram.Bot.Polling.ReceiverOptions
            {
                AllowedUpdates = { }
            };
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cts.Token
            );
            var me = await _botClient.GetMe();
            Console.WriteLine($"@{me.Username} запущен, отправь URL туда");
            Console.ReadLine();
            cts.Cancel();
        }
        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, System.Threading.CancellationToken token)
        {
            if (update.Message == null || update.Message.Text == null)
                return;
            string text = update.Message.Text.Trim();
            if (!Uri.IsWellFormedUriString(text, UriKind.Absolute))
            {
                await bot.SendMessage(update.Message.Chat.Id, "Неправильный URL");
                return;
            }
            var sentMsg = await bot.SendMessage(update.Message.Chat.Id, "Создаю скриншот...");
            try
            {
                string file = await _screenshotService.CaptureWebsiteScreenshot(text);
                using var fs = System.IO.File.OpenRead(file);
                await bot.SendPhoto(update.Message.Chat.Id, new Telegram.Bot.Types.InputFileStream(fs));
                System.IO.File.Delete(file);
            }
            catch { }
            await bot.DeleteMessage(update.Message.Chat.Id, sentMsg.MessageId);
        }
        private Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, System.Threading.CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
