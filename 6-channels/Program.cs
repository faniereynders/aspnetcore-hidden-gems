using System.Collections.Concurrent;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
var _channel = Channel.CreateUnbounded<string>();

builder.Services.AddSingleton(_channel);

var app = builder.Build();

app.MapControllers();
app.UseStaticFiles().UseDefaultFiles();

app.Run();
