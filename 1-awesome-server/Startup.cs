using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddMvc(o => o.EnableEndpointRouting = false);

        public void Configure(IApplicationBuilder app) => app.UseMvc();
    }
}
