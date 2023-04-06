using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
namespace Microsoft.AspNetCore.Hosting;

public static class ServerExtensions
{
    public static IWebHostBuilder UseTorenvalk(this IWebHostBuilder hostBuilder, Action<TorenvalkOptions> options)
    {
        return hostBuilder.ConfigureServices(services =>
        {
            services.Configure(options);
            services.AddSingleton<IServer, TorenvalkServer>();
            services.AddSingleton<FolderWatcher>();
        });
    }
}


