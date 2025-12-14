namespace ScreenshotBot.Services
{
    public static class TempFileHelper
    {
        public static string GetTempFile(string extension = ".png")
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
            return tempPath;
        }
        public static void DeleteTempFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch { }
        }
    }
}
