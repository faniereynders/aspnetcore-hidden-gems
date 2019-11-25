using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace ExternalMVC
{
    public class Program
    {
        public static void Main(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddExternalMvc(args[0]);
                })
                .Configure(app => app.UseMvc())
                .Build()
                .Run();
    }
}
