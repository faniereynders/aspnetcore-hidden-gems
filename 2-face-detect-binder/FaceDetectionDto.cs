using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Reflection;

namespace AwesomeApi
{
    [ModelBinder(typeof(AwesomeModelBinder))]
    public class FaceDetectionDto
    {
        public byte[] Contents { get; set; }
        public FaceRectangleDto[] Faces { get; set; }

        public static async ValueTask<FaceDetectionDto?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
        {
            var client = httpContext.RequestServices.GetService<FaceDetectionClient>();
            const string propertyName = "Photo";
            var valueProviderResult = httpContext.Request.Form[propertyName].FirstOrDefault();
            if (!string.IsNullOrEmpty(valueProviderResult))
            {
                var bytes = Convert.FromBase64String(valueProviderResult);
                var response = await client.GetFacesAsync(bytes);
                var faces = response.Select(i => i.FaceRectangle).ToArray();
                var result = new FaceDetectionDto
                {
                    Contents = bytes,
                    Faces = faces
                };
                return result;
            }

            return null;
           
        
        }
    }
}
