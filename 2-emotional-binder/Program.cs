using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AwesomeApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
    ////        var config = new ConfigurationBuilder()
    ////// Call additional providers here as needed.
    ////// Call AddCommandLine last to allow arguments to override other configuration.
    ////.AddCommandLine(args)
    ////.Build();
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}
