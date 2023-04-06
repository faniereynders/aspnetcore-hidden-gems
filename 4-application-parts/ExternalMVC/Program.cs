using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var url = builder.Configuration.GetValue<string>("ApiAssemblyDownloadUrl");
var bytes = await new HttpClient().GetByteArrayAsync(url);
var assembly = Assembly.Load(bytes);

builder.Services
    .AddMvc(o => o.EnableEndpointRouting = false)
    .AddApplicationPart(assembly);

var app = builder.Build();

app.UseMvc();
app.Run();    

