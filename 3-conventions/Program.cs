using AwesomeConventions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<IPeopleRepository, PeopleRepository>()
    .AddControllers(o => o.Conventions.Add(new AwesomeConvention()));

var app = builder.Build();

app.MapControllers();

app.Run();