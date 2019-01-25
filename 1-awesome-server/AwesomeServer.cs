using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeServer
{
    public class AwesomeServer : IServer
    {
        private readonly string inboxPath;
        private readonly string outboxPath;
        private readonly IServiceProvider serviceProvider;

        public AwesomeServer(IOptions<AwesomeServerOptions> options, IServiceProvider serviceProvider)
        {
            this.inboxPath = options.Value.InboxPath;
            this.outboxPath = options.Value.OutboxPath;

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

            Features.Set<IHttpRequestFeature>(new HttpRequestFeature());
            Features.Set<IHttpResponseFeature>(new HttpResponseFeature());
            Features.Set<IServiceProvidersFeature>(new ServiceProvidersFeature() { RequestServices = serviceProvider });
            Features.Set<IServerAddressesFeature>(serverAddressesFeature);

            this.serviceProvider = serviceProvider;
        }

        public IFeatureCollection Features { get; } = new FeatureCollection();

        public void Dispose() { }

        public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var watcher = new AwesomeFolderWatcher<TContext>(application, Features, inboxPath, outboxPath);
                watcher.Watch();
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
