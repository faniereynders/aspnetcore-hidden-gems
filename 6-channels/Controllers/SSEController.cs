using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

public class SSEController : Controller
{
    private readonly Channel<string> channel;
    private readonly ConcurrentDictionary<Guid, ChannelWriter<string>> subscribers;
    private readonly List<ChannelReader<string>> _readers = new List<ChannelReader<string>>();

    // private readonly Channel<string> _eventChannel = Channel.CreateUnbounded<string>();
    public SSEController(Channel<string> channel, ConcurrentDictionary<Guid, ChannelWriter<string>> subscribers)
    {
        this.channel = channel;
        this.subscribers = subscribers;
    }

    [HttpGet("~/sse")]
    public async Task<IActionResult> Stream()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");
        var clientId = Guid.NewGuid();


        var cancellationToken = HttpContext.RequestAborted;

        await foreach (var data in channel.Reader.ReadAllAsync(cancellationToken))
        {
            var message = $"data: {data}\n\n";
            var bytes = Encoding.UTF8.GetBytes(message);

            await Response.Body.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }

        return new EmptyResult();
    }

    [HttpPost("~/sse")]
    public async Task SendEvent(string data)
    {
        await channel.Writer.WriteAsync(data);
        //channel.Writer.Complete();
    }

   
}
