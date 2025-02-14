using MarsRoverAPI.Models;

namespace MarsRoverAPI.Services
{
    public static class DirectionService
    {
        /// <summary>
        /// Returns the direction after turning right.
        /// </summary>
        /// <param name="direction">The current direction.</param>
        /// <returns>The new direction after a right turn.</returns>
        public static Direction TurnRight(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        /// <summary>
        /// Returns the direction after turning left.
        /// </summary>
        /// <param name="direction">The current direction.</param>
        /// <returns>The new direction after a left turn.</returns>
        public static Direction TurnLeft(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.West,
                Direction.West => Direction.South,
                Direction.South => Direction.East,
                Direction.East => Direction.North,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }

}
