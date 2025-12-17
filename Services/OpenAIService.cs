using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FitnessCenter.Models;

namespace FitnessCenter.Services
{
    public class OpenAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"] ?? string.Empty;
        }

        public async Task<FitnessPlanResult> GenerateFitnessPlanAsync(FitnessPlanRequest request)
        {
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "YOUR_OPENAI_API_KEY_HERE")
            {
                return new FitnessPlanResult { PlanContent = "Hata: OpenAI API Anahtarı eksik. Lütfen appsettings.json dosyasını yapılandırın." };
            }

            var prompt = $"{request.Age} yaşında {request.Gender}, " +
                         $"Boy: {request.Height}cm, Kilo: {request.Weight}kg, Vücut Tipi: {request.BodyType} olan biri için detaylı bir fitness ve diyet planı oluştur. " +
                         $"Hedef: {request.Goal}. Cevabı Türkçe olarak ve formatlı Markdown biçiminde ver.";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Sen profesyonel bir fitness antrenörü ve diyetisyensin." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 2000 
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new FitnessPlanResult { PlanContent = $"OpenAI API hatası: {response.StatusCode} - {error}" };
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);
            var planContent = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return new FitnessPlanResult { PlanContent = planContent ?? "İçerik oluşturulamadı." };
        }

        public async Task<VisualizationResult> GenerateVisualizationAsync(VisualizationRequest request)
        {
             if (string.IsNullOrEmpty(_apiKey) || _apiKey == "YOUR_OPENAI_API_KEY_HERE")
            {
                return new VisualizationResult { ImageUrl = "" }; 
            }

            var requestBody = new
            {
                prompt = $"{request.Description}, high quality, realistic fitness transformation, photorealistic, 4k", // Keep image prompt in English as DALL-E understands it better usually, or we can translate user input. Let's keep specific style keywords in English.
                n = 1,
                size = "512x512"
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/images/generations", content);
             if (!response.IsSuccessStatusCode)
            {
                 // For now return empty, or handle error better
                 return new VisualizationResult { ImageUrl = "" };
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);
            var imageUrl = doc.RootElement.GetProperty("data")[0].GetProperty("url").GetString();

            return new VisualizationResult { ImageUrl = imageUrl ?? "" };
        }
    }
}
