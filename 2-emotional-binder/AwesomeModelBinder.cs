using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwesomeApi
{
    public class AwesomeModelBinder : IModelBinder
    {
        private readonly EmotionalClient client;

        public AwesomeModelBinder(EmotionalClient client) => this.client = client;

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            const string propertyName = "Photo";
            var valueProviderResult = bindingContext.ValueProvider.GetValue(propertyName);

            var base64Value = valueProviderResult.FirstValue;
            if (!string.IsNullOrEmpty(base64Value))
            {
                var bytes = Convert.FromBase64String(base64Value);
                var emotionResult = await client.GetEmotionResultAsync(bytes);
                var scores = emotionResult.Select(i => i.FaceAttributes.Emotion).ToArray();
                var result = new EmotionalPhotoDto
                {
                    Contents = bytes,
                    Scores = scores
                };
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            await Task.FromResult(Task.CompletedTask);
        }
        
    }
}
