using Microsoft.AspNetCore.Mvc;
using MarsRoverAPI.Models;
using MarsRoverAPI.Services;

namespace MarsRoverAPI.Controllers
{
    /// <summary>
    /// API Controller for the rover.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoverController(Rover rover, PlanetGrid grid, CommandService commandService) : ControllerBase
    {
        private readonly Rover _rover = rover;
        private readonly PlanetGrid _grid = grid;
        private readonly CommandService _commandService = commandService;

        /// <summary>
        /// Processes a sequence of commands to control the rover.
        /// Acceptable commands are 'f' (forward), 'b' (backward), 'l' (turn left), and 'r' (turn right).
        /// If an obstacle is encountered, further commands are ignored.
        /// </summary>
        /// <param name="commands">An array of command characters.</param>
        /// <returns>
        /// A 200 OK response with the rover's current direction, position, and a grid representation,
        /// or a 400 Bad Request response if no commands are provided or if the commands are invalid.
        /// </returns>
        [HttpPost("command")]
        public IActionResult Command([FromBody] char[] commands)
        {
            if (commands == null || commands.Length == 0)
                return BadRequest("No command received in the request.");

            if (!_commandService.ValidateCommands(commands))
                return BadRequest("Unknown commands were found, the request was cancelled.");

            _rover.ResetObstacleFlag();

            foreach (char command in commands)
            {
                switch (command)
                {
                    case 'f':
                    case 'F':
                        _rover.MoveForward(1);
                        break;
                    case 'b':
                    case 'B':
                        _rover.MoveBackward(1);
                        break;
                    case 'l':
                    case 'L':
                        _rover.TurnLeft();
                        break;
                    case 'r':
                    case 'R':
                        _rover.TurnRight();
                        break;
                }

                if (_rover.ObstacleEncountered)
                    break;
            }

            var roverPosition = _rover.GetPosition();
            var roverDirection = _rover.GetFacingDirection();

            string header = $"Direction: {roverDirection}, Position: ({roverPosition.X}, {roverPosition.Y})";
            if (_rover.ObstacleEncountered)
                header += "\n⚠️ Obstacle encountered!";

            return Ok(header + Environment.NewLine + _grid.DrawGrid(roverPosition));
        }
    }
}
