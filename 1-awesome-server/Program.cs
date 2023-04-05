

using AwesomeServer;
using AwesomeServer.Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

//var app = WebHost.CreateDefaultBuilder(args)
//    .UseAwesomeServer(o =>
//    {
//        o.InboxPath = @".\process\inbox";
//        o.OutboxPath = @".\process\outbox";
//    })
//    .UseStartup<Startup>()
//    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseAwesomeServer(o =>
{
    o.InboxPath = @".\process\inbox";
    o.OutboxPath = @".\process\outbox";
});

var app = builder.Build();

app.MapGet("/foo", () => "Hello FOO!");






app.Run();
