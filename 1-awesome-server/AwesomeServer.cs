using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

public class AwesomeServer : IServer
{
    private readonly AwesomeFolderWatcher awesomeFolderWatcher;

    public AwesomeServer(IOptions<AwesomeServerOptions> options, IServiceProvider serviceProvider, AwesomeFolderWatcher awesomeFolderWatcher)
    {
        var inboxPath = options.Value.InboxPath;
        var outboxPath = options.Value.OutboxPath;

        if (!Directory.Exists(inboxPath))
        {
            Directory.CreateDirectory(inboxPath);
        }
        if (!Directory.Exists(outboxPath))
        {
            Directory.CreateDirectory(outboxPath);
        }

        var serverAddressesFeature = new ServerAddressesFeature();
        var inboxLocation = new DirectoryInfo(inboxPath).FullName;
        serverAddressesFeature.Addresses.Add(inboxLocation);

        Features.Set<IServiceProvidersFeature>(new ServiceProvidersFeature() { RequestServices = serviceProvider });
        Features.Set(serverAddressesFeature);

        this.awesomeFolderWatcher = awesomeFolderWatcher;
    }

    public IFeatureCollection Features => new FeatureCollection();

    public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        => awesomeFolderWatcher.WatchAsync(application, Features);

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public void Dispose() { }
}
