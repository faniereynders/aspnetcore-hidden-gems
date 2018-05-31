using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeServer
{
    public class AwesomeFolderWatcher<TContext>
    {
        private readonly FileSystemWatcher watcher;
        private readonly IHttpApplication<TContext> application;
        private readonly IFeatureCollection features;
        private readonly string outPath;

        public AwesomeFolderWatcher(IHttpApplication<TContext> application, IFeatureCollection features, string inPath, string outPath)
        {
            this.watcher = new FileSystemWatcher(inPath)
            {
                EnableRaisingEvents = true
            };
            this.application = application;
            this.features = features;
            this.outPath = outPath;
        }
        public void Watch()
        {
            watcher.Created += async (sender, e) =>
            {
                var context = (HostingApplication.Context)(object)application.CreateContext(features);
                context.HttpContext = new AwesomeHttpContext(features, e.FullPath, outPath);
                await application.ProcessRequestAsync((TContext)(object)context);
                context.HttpContext.Response.OnCompleted(null, null);
            };

            Task.Run(() => watcher.WaitForChanged(WatcherChangeTypes.All));
        }
    }

}
