using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace AwesomeServer
{
    public class AwesomeFolderWatcher 
    {
        private readonly FileSystemWatcher watcher;
        private readonly IOptions<AwesomeServerOptions> options;

        public AwesomeFolderWatcher(IOptions<AwesomeServerOptions> options)
        {
            this.watcher = new FileSystemWatcher(options.Value.InboxPath)
            {
                EnableRaisingEvents = true
            };
            this.options = options;
        }
        public Task WatchAsync<TContext>(IHttpApplication<TContext> application, IFeatureCollection features)
        {
            watcher.Created += async (sender, e) =>
            {
                var request = CreateHttpRequestFromFile(e.FullPath);
                features.Set(request);
                features.Set<IHttpResponseFeature>(new HttpResponseFeature());
                features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(new MemoryStream()));

                var context = application.CreateContext(features);
                await application.ProcessRequestAsync(context);

                WriteOutputToFile(features, e.Name);

            };

            Task.Run(() => watcher.WaitForChanged(WatcherChangeTypes.All));
            return Task.CompletedTask;
        }

        private void WriteOutputToFile(IFeatureCollection features, string fileName)
        {
            var response = features.Get<IHttpResponseFeature>();
            var responseBody = features.Get<IHttpResponseBodyFeature>();

            var fname = BuildOutputFileName(fileName, response);

            using (var reader = new StreamReader(responseBody.Stream))
            using (var fs = new FileStream(fname, FileMode.CreateNew))
            {
                responseBody.Stream.Seek(0, SeekOrigin.Begin);
                responseBody.Stream.CopyTo(fs);
                responseBody.Stream.Flush();
                responseBody.Stream.Dispose();
                fs.Close();
            }
        }

        private string BuildOutputFileName(string fileName, IHttpResponseFeature httpResponseFeature)
        {
            var outFileName = Path.Combine(options.Value.OutboxPath, fileName);

            var info = new FileInfo(outFileName);
            var ext = info.Extension;
            if (httpResponseFeature.Headers["Content-Type"].Any(c => c.Contains("json")))
            {
                ext = ".json";
            }
            else if (httpResponseFeature.Headers["Content-Type"].Any(c => c.Contains("png"))) 
            {
                ext = ".png";
            }

            return Path.Combine(info.Directory.FullName, DateTime.Now.Ticks.ToString() + "_" + info.Name.Replace(info.Extension, ext));

        }
        private static IHttpRequestFeature CreateHttpRequestFromFile(string filename)
        {
            var fileRequest = File.ReadAllText(filename).Split(' ');

            return new HttpRequestFeature()
            {
                Method = fileRequest[0],
                Path = fileRequest[1]
            };
        }
    }





}
