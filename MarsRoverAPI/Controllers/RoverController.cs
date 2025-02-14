using Microsoft.AspNetCore.Mvc;
using MarsRoverAPI.Models;
using MarsRoverAPI.Services;
using MarsRoverAPI.Models.Exceptions;

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
            int statusCode = 200;
            string errorMessage = string.Empty;

            if (commands == null || commands.Length == 0)
                return BadRequest("No command received in the request.");

            if (!_commandService.ValidateCommands(commands))
                return BadRequest("Unknown commands were found, the request was cancelled.");

            try
            {
                foreach (char command in commands)
                {
                    switch (char.ToLower(command))
                    {
                        case 'f':
                            _rover.MoveForward(1);
                            break;
                        case 'b':
                            _rover.MoveBackward(1);
                            break;
                        case 'l':
                            _rover.TurnLeft();
                            break;
                        case 'r':
                            _rover.TurnRight();
                            break;
                    }
                }

            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (ObstacleEncounteredException ex)
            {
                statusCode = 409;
                errorMessage = ex.Message;
            }

            var roverPosition = _rover.GetPosition();
            var roverDirection = _rover.GetFacingDirection();

            string header = $"Direction: {roverDirection}, Position: ({roverPosition.X}, {roverPosition.Y})";

            return StatusCode(statusCode, header + Environment.NewLine + errorMessage + Environment.NewLine + _grid.DrawGrid(roverPosition));
        }
    }
}
