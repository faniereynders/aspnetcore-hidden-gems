using Microsoft.AspNetCore;
using BasicExample.Models;


WebHost
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddMvc(o => o.EnableEndpointRouting = false)
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

