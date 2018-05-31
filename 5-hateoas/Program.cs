using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using BasicExample.Models;
using Microsoft.AspNetCore.Builder;

namespace BasicExample
{
    public class Program
    {
        public static void Main(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(services => {
                    services
                        .AddMvc()
                        .AddHateoas(options =>
                        {
                            options
                                .AddLink<PersonDto>("get-person", p => new { id = p.Id })
                                .AddLink<List<PersonDto>>("create-person")
                                .AddLink<PersonDto>("update-person", p => new { id = p.Id })
                                .AddLink<PersonDto>("delete-person", p => new { id = p.Id });
                        });
                    })
                .Configure(app => app.UseMvc())
                .Build()
                .Run();
    }
}
