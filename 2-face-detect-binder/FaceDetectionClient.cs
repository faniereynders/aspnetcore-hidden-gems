using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwesomeApi
{
    public class FaceDetectionClient
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public FaceDetectionClient(HttpClient httpClient, IConfiguration configuration, JsonSerializerOptions jsonSerializerOptions)
        {
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configuration["SubscriptionKey"]);
            this.httpClient = httpClient;
            this.jsonSerializerOptions = jsonSerializerOptions;
        }
        public async Task<FaceDetectionResultDto[]> GetFacesAsync(byte[] byteArray)
        {
            
            
            var uri = $"https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";
            using (var content = new ByteArrayContent(byteArray))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FaceDetectionResultDto[]>(responseContent, jsonSerializerOptions);
                return result;
            }
        }
    }
}
