using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwesomeApi
{
    public class AwesomeModelBinder : IModelBinder
    {
        private readonly FaceDetectionClient client;

        public AwesomeModelBinder(FaceDetectionClient client) => this.client = client;

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            const string propertyName = "Photo";
            var valueProviderResult = bindingContext.ValueProvider.GetValue(propertyName);

            var base64Value = valueProviderResult.FirstValue;
            if (!string.IsNullOrEmpty(base64Value))
            {
                var bytes = Convert.FromBase64String(base64Value);
                var response = await client.GetFacesAsync(bytes);
                var faces = response.Select(i => i.FaceRectangle).ToArray();
                var result = new FaceDetectionDto
                {
                    Contents = bytes,
                    Faces = faces
                };
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            await Task.FromResult(Task.CompletedTask);
        }
        
    }
}
