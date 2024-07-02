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
        {_apiKey = apiKey; }

        public async Task<string> GenerateAnonimizedName(string originalName, string gender)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var prompt = $"Generate an anonymized {gender} name for {originalName}";

            var requestBody = new
            {
                prompt = prompt,
                max_tokens = 10
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/engines/davinci-codex/completions", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JsonDocument.Parse(responseBody);

            var generatedName = responseJson.RootElement.GetProperty("choices")[0].GetProperty("text").ToString().Trim();

            return generatedName;
        }
    }
}
