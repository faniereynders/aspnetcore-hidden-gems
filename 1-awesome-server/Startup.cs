using Microsoft.AspNetCore.Builder;
public class Startup
{
    public void ConfigureServices(IServiceCollection services) =>
        services.AddMvc();

    public void Configure(IApplicationBuilder app) => app.UseMvc();
}
