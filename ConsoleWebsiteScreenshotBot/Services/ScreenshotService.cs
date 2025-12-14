using PuppeteerSharp;

namespace ScreenshotBot.Services
{
    public class ScreenshotService
    {
        public ScreenshotService() { }
        public async Task<string> CaptureWebsiteScreenshot(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL пустое", nameof(url));
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            var launchOptions = new LaunchOptions
            {
                Headless = true
            };
            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();
            if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                url = "https://" + url;
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle2);
            string tempFile = Path.Combine(Path.GetTempPath(), $"screenshot_{Guid.NewGuid()}.png");
            await page.ScreenshotAsync(tempFile, new ScreenshotOptions
            {
                FullPage = true
            });
            return tempFile;
        }
    }
}
