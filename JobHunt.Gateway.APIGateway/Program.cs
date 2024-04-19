using JobHunt.Gateway.APIGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Jwt Authentication Configurations
builder.AddAppAuthentication();

builder.Services.AddCors();

// Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// CORS Policy
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseOcelot().GetAwaiter().GetResult();

app.Run();