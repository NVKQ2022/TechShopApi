using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TechShop_API_backend_.Service
{
    public class ImageService
    {
        private readonly string _imageKitPrivateKey;
        private readonly string _imageKitPublicKey;
        private readonly string _imageKitUrlEndpoint;
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif"
        };

        public ImageService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _imageKitPrivateKey = configuration["ImageKit:PrivateKey"]
                ?? Environment.GetEnvironmentVariable("ImageKit__PrivateKey")
                ?? string.Empty;
            _imageKitPublicKey = configuration["ImageKit:PublicKey"]
                ?? Environment.GetEnvironmentVariable("ImageKit__PublicKey")
                ?? string.Empty;
            _imageKitUrlEndpoint = configuration["ImageKit:UrlEndpoint"]
                ?? Environment.GetEnvironmentVariable("ImageKit__UrlEndpoint")
                ?? string.Empty;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded");
            }

            // Max 10MB limit
            if (file.Length > 10 * 1024 * 1024)
            {
                throw new ArgumentException("File size exceeds 10MB limit");
            }

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Only image files (.jpg, .jpeg, .png, .webp, .gif) are allowed");
            }

            return await UploadToImageKit(file);
        }

        private async Task<string> UploadToImageKit(IFormFile file)
        {
            if (string.IsNullOrEmpty(_imageKitPrivateKey))
            {
                throw new InvalidOperationException("ImageKit PrivateKey is not configured.");
            }

            var client = _httpClientFactory.CreateClient();

            var credentials = $"{_imageKitPrivateKey}:";
            var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

            using var formData = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            using var streamContent = new StreamContent(fileStream);

            formData.Add(streamContent, "file", file.FileName);
            formData.Add(new StringContent(file.FileName), "fileName");

            var response = await client.PostAsync("https://upload.imagekit.io/api/v1/files/upload", formData);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                return result?.url ?? string.Empty;
            }
            else
            {
                throw new Exception($"Image upload failed: {responseContent}");
            }
        }
    }
}
