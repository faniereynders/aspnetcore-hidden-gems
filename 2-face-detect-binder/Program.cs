using AwesomeApi;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
});
builder.Services.AddHttpClient<FaceDetectionClient>();

var app = builder.Build();

app.MapPost("/api/example",(FaceDetectionDto request)=> Results.Ok(request.Faces));
app.Run();


