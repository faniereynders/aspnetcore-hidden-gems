using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Reflection;

namespace ExternalMVC
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcBuilder AddExternalMvc(this IServiceCollection services, string uri)
        {
            var bytes = new HttpClient().GetByteArrayAsync(uri).GetAwaiter().GetResult();
            var assembly = Assembly.Load(bytes);

            var builder = services
                            .AddMvc()
                            .AddApplicationPart(assembly);

            return builder;
        }
    }
}
