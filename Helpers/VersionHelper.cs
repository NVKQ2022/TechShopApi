namespace TechShopApi.Helpers
{
    public class VersionHelper
    {
        public string? GetBuildInfo(string key)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "buildinfo.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split('=', 2);
                        if (parts.Length == 2)
                        {
                            return parts[1].Trim();
                        }
                    }
                }
            }

            return null;
        }
    }
}
