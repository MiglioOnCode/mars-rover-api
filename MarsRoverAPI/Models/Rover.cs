namespace MarsRoverAPI.Models
{
    /// <summary>
    /// Represents a Mars rover that can move within a grid, turn, and detect obstacles.
    /// </summary>
    public class Rover(Position position, Direction facingDirection, PlanetGrid grid)
    {
        private Position _position = position;
        private Direction _facingDirection = facingDirection;
        private readonly PlanetGrid _grid = grid;

        public bool ObstacleEncountered { get; private set; } = false;
        public void ResetObstacleFlag() => ObstacleEncountered = false;

        public Position GetPosition() => _position;
        public Direction GetFacingDirection() => _facingDirection;

        /// <summary>
        /// Rotates the rover 90 degrees to the right.
        /// </summary>
        public void TurnRight()
        {
            _facingDirection = _facingDirection switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new InvalidOperationException("Invalid direction."),
            };
        }

        /// <summary>
        /// Rotates the rover 90 degrees to the left.
        /// </summary>
        public void TurnLeft()
        {
            _facingDirection = _facingDirection switch
            {
                Direction.North => Direction.West,
                Direction.West => Direction.South,
                Direction.South => Direction.East,
                Direction.East => Direction.North,
                _ => throw new InvalidOperationException("Invalid direction."),
            };
        }

        /// <summary>
        /// Moves the rover forward by the specified number of steps.
        /// </summary>
        /// <param name="steps">The number of steps to move forward.</param>
        public void MoveForward(int steps) => Move(steps);

        /// <summary>
        /// Moves the rover backward by the specified number of steps.
        /// </summary>
        /// <param name="steps">The number of steps to move backward.</param>
        public void MoveBackward(int steps) => Move(-steps);

        /// <summary>
        /// Moves the rover by the specified number of steps, taking into account obstacles and grid wrapping.
        /// If an obstacle is encountered, movement is aborted.
        /// </summary>
        /// <param name="steps">The number of steps to move (positive for forward, negative for backward).</param>
        private void Move(int steps)
        {
            // Determine the movement factor: 1 for forward, -1 for backward.
            int movementFactor = steps > 0 ? 1 : -1;

            for (int i = 0; i < Math.Abs(steps); i++)
            {
                Position nextPosition = _facingDirection switch
                {
                    Direction.North => new Position(_position.X, _position.Y - movementFactor),
                    Direction.South => new Position(_position.X, _position.Y + movementFactor),
                    Direction.East => new Position(_position.X + movementFactor, _position.Y),
                    Direction.West => new Position(_position.X - movementFactor, _position.Y),
                    _ => throw new InvalidOperationException("Invalid direction.")
                };

                // Apply grid wrapping to ensure the rover stays within the grid boundaries.
                nextPosition = _grid.WrapPosition(nextPosition);

                if (_grid.IsObstacleAt(nextPosition))
                {
                    ObstacleEncountered = true;
                    break;
                }

                _position = nextPosition;
            }
        }
    }
}
