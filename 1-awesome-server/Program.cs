using Microsoft.AspNetCore;

var builder = WebHost.CreateDefaultBuilder<Startup>(args);

builder.UseAwesomeServer(o =>
{
    o.InboxPath = @".\process\inbox";
    o.OutboxPath = @".\process\outbox";
});


builder.Build().Run();
