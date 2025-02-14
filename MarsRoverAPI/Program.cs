using MarsRoverAPI.Models;
using MarsRoverAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Retrieve the PlanetGrid configuration section
var planetGridSection = builder.Configuration.GetSection("Settings:PlanetGrid");
if (!planetGridSection.Exists())
    throw new InvalidOperationException("Missing 'PlanetGrid' configuration section in appsettings.json.");

// Retrieve and validate configuration values.
int width = planetGridSection.GetValue<int>("Width");
int height = planetGridSection.GetValue<int>("Height");
int numberOfObstacles = planetGridSection.GetValue<int>("NumberOfObstacles");

if (width <= 0)
    throw new InvalidOperationException("The grid width must be greater than zero.");

if (height <= 0)
    throw new InvalidOperationException("The grid height must be greater than zero.");

builder.Services.AddSingleton(serviceProvider => new PlanetGrid(width, height, numberOfObstacles));
builder.Services.AddSingleton(serviceProvider => new Rover(new Position(0, 0), Direction.North, serviceProvider.GetRequiredService<PlanetGrid>()));
builder.Services.AddSingleton<CommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
