using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using System.Text;

public class AwesomeFolderWatcher
{
    private readonly FileSystemWatcher watcher;
    private readonly IOptions<AwesomeServerOptions> options;
    private readonly ILogger logger;

    public AwesomeFolderWatcher(IOptions<AwesomeServerOptions> options, ILoggerFactory logger)
    {
        this.watcher = new FileSystemWatcher(options.Value.InboxPath)
        {
            EnableRaisingEvents = true
        };
        this.options = options;
        this.logger = logger.CreateLogger(nameof(AwesomeServer));
    }
    public Task WatchAsync<TContext>(IHttpApplication<TContext> application, IFeatureCollection features)
    {
        var oDir = new DirectoryInfo(options.Value.OutboxPath);
        watcher.Created += async (sender, e) =>
        {
            logger.Log(LogLevel.Information, $"New file trigger: {e.Name}");
            var request = CreateHttpRequestFromFile(e.FullPath);
            features.Set<IHttpRequestFeature>(request);
            features.Set<IHttpResponseFeature>(new HttpResponseFeature());
            features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(new MemoryStream()));

            var context = application.CreateContext(features);
            await application.ProcessRequestAsync(context);

            WriteOutputToFile(features, e.Name);
            logger.Log(LogLevel.Information, $"{e.Name} processed to folder: {oDir.FullName}");
            File.Delete(e.FullPath);
            application.DisposeContext(context, null);
        };

        Task.Run(() => watcher.WaitForChanged(WatcherChangeTypes.All));
        var iDir = new DirectoryInfo(options.Value.InboxPath);
        logger.Log(LogLevel.Information, $"Listening for files in folder: {iDir.FullName}");
        return Task.CompletedTask;
    }

    private void WriteOutputToFile(IFeatureCollection features, string fileName)
    {
        var response = features.Get<IHttpResponseFeature>();
        var responseBody = features.Get<IHttpResponseBodyFeature>();
        var key = Guid.NewGuid().ToString();
        

        using (var reader = new StreamReader(responseBody.Stream))
        {
            var sb = new StringBuilder();
            sb.AppendLine($"HTTP: {response.StatusCode}");
            response.Headers.ToList().ForEach(h =>
            {
                sb.AppendLine($"{h.Key}: {h.Value}");
            });
            sb.AppendLine("---");
            responseBody.Stream.Seek(0, SeekOrigin.Begin);
            sb.AppendLine(reader.ReadToEnd());
            
            var fname = BuildOutputFileName(key, fileName, response);
            
            File.WriteAllText(fname, sb.ToString());
        }

    }

    private string BuildOutputFileName(string key, string fileName, IHttpResponseFeature httpResponseFeature)
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

        return Path.Combine(info.Directory.FullName, key + "_" + info.Name.Replace(info.Extension, ext));

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
