using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProtectedDba.Services
{
    public class OpenAiService
    {
        private readonly string _apiKey; //at JSON

        public OpenAiService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey)); //Ensures that _apiKey is not null and prevents future errors
        }

        public async Task<string> GenerateAnonimizedName(string originalName, string gender)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var prompt = $"Generate an anonymized {gender} name for {originalName}";

            var requestBody = new
            {
                model = "gpt-3.5-turbo", 
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                max_tokens = 10,
                temperature = 0.7f
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JsonDocument.Parse(responseBody);

            var generatedName = responseJson.RootElement.GetProperty("choices")[0]
                .GetProperty("message").GetProperty("content").ToString().Trim();

            return generatedName;
        }
    }
}
