namespace TechShopApi.Helpers
{
    public class VersionHelper
    {
        public string GetBuildInfo(string key)
        {
            // Path to the buildinfo.txt file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "buildinfo.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith(key))
                    {
                        // Split each line by '=' and return the value after '='
                        return line.Split('=')[1];
                    }
                }
            }

            return null; // Return null if the file doesn't exist or key not found
        }
    }
}
