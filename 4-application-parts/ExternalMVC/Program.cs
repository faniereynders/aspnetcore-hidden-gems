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
                    services.AddExternalMvc("https://www.dropbox.com/s/ijk3g703oabh2se/Api.dll?dl=1");
                })
                .Configure(app => app.UseMvc())
                .Build()
                .Run();
    }
}
