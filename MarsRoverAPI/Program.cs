using MarsRoverAPI.Models;
using MarsRoverAPI.Services;;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Retrieve the PlanetGrid configuration section
var planetGridSection = builder.Configuration.GetSection("Settings:PlanetGrid");
var roverSection = builder.Configuration.GetSection("Settings:Rover");

if (!planetGridSection.Exists()) throw new InvalidOperationException("Missing 'PlanetGrid' configuration section in appsettings.json.");
if (!roverSection.Exists()) throw new InvalidOperationException("Missing 'Rover' configuration section in appsettings.json.");

// Retrieve and validate configuration values.
int width = planetGridSection.GetValue<int>("Width");
int height = planetGridSection.GetValue<int>("Height");
int numberOfObstacles = planetGridSection.GetValue<int>("NumberOfObstacles");


Position startingPosition = new(roverSection.GetValue<int>("StartingX"), roverSection.GetValue<int>("StartingY"));

Direction startingDirection = roverSection.GetValue<Direction>("StartingDirection");

builder.Services.AddSingleton(serviceProvider => new PlanetGrid(width, height, GenerateObstacles(numberOfObstacles, startingPosition, width, height)));
builder.Services.AddSingleton(serviceProvider => new Rover(startingPosition, startingDirection, serviceProvider.GetRequiredService<PlanetGrid>()));
builder.Services.AddSingleton<CommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Generates a list of obstacles ensuring none coincide with the starting position of the rover.
/// </summary>
/// <param name="numberOfObstacles">Number of obstacles to generate.</param>
/// <param name="roverPosition">The starting position of the rover.</param>
/// <param name="gridWidth">Grid width.</param>
/// <param name="gridHeight">Grid height.</param>
/// <returns>A list of positions representing obstacles.</returns>
static List<Position> GenerateObstacles(int numberOfObstacles, Position roverPosition,int gridWidth, int gridHeight)
{
    var obstacles = new List<Position>();
    var random = new Random();

    numberOfObstacles = Math.Min(numberOfObstacles, gridWidth * gridHeight - 1);

    for (int i = 0; i < numberOfObstacles; i++)
    {
        Position obstacle;
        do
        {
            obstacle = new Position(random.Next(0, gridWidth), random.Next(0, gridHeight));
        } while ((obstacle.X == roverPosition.X && obstacle.Y == roverPosition.Y) ||
               obstacles.Any(o => o.X == obstacle.X && o.Y == obstacle.Y));

        obstacles.Add(obstacle);
    }

    return obstacles;
}