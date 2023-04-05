using AwesomeApi;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(o => o.EnableEndpointRouting = false);
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
});
builder.Services.AddHttpClient<FaceDetectionClient>();

var app = builder.Build();

app.UseMvc();

app.MapPost("/api/example/minapi",(FaceDetectionDto request)=> Results.Ok(request.Faces));
app.Run();


