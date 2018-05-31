using AwesomeServer.Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AwesomeServer
{
    class Program
    {
        static void Main(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseAwesomeServer(o =>
                {
                    o.InboxPath = @".\process\inbox";
                    o.OutboxPath = @".\process\outbox";
                })
                .UseStartup<Startup>()
                .Build()
                .Run();
    }
}
