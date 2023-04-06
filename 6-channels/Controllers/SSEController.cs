using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

public class SSEController : Controller
{
    private readonly Channel<string> channel;

    public SSEController(Channel<string> channel)
    {
        this.channel = channel;
        
    }

    [HttpGet("~/sse")]
    public async Task<IActionResult> Stream()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

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
    }

   
}
