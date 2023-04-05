using System.Collections.Concurrent;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var _channel = Channel.CreateBounded<string>(new BoundedChannelOptions(10)
{
    SingleReader = false,
    SingleWriter = true
});



builder.Services.AddSingleton(_channel);

var app = builder.Build();

app.MapControllers();
app.UseStaticFiles().UseDefaultFiles();

app.Run();
