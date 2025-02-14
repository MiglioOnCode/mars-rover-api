using System.Text;

namespace MarsRoverAPI.Models
{
    /// <summary>
    /// Represents the grid of the planet, including its dimensions and obstacles.
    /// </summary>
    public class PlanetGrid
    {
        public int Width { get; }
        public int Height { get; }
        public List<Position> Obstacles { get; }

        public PlanetGrid(int width, int height, List<Position> obstacles)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException($"Invalid grid dimensions: width must be greater than zero and height must be greater than zero. Received: width={width}, height={height}");

            Width = width;
            Height = height;
            Obstacles = obstacles;
        }

        /// <summary>
        /// Wraps the specified position so that it stays within the grid boundaries.
        /// </summary>
        /// <param name="position">The original position.</param>
        /// <returns>
        /// A new <see cref="Position"/> representing the wrapped coordinates within the grid.
        /// </returns>
        public Position WrapPosition(Position position)
        {
            int wrappedX = (position.X + Width) % Width;
            int wrappedY = (position.Y + Height) % Height;
            return new Position(wrappedX, wrappedY);
        }

        /// <summary>
        /// Checks if there is an obstacle at the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>
        /// <c>true</c> if an obstacle exists at the given position; otherwise, <c>false</c>.
        /// </returns>
        public bool IsObstacleAt(Position position)
        {
            return Obstacles.Any(o => o.X == position.X && o.Y == position.Y);
        }

        /// <summary>
        /// Generates a string representation of the grid using emojis. 
        /// The grid includes a border, displays the rover's position, obstacles, and empty cells.
        /// </summary>
        /// <param name="roverPosition">The current position of the rover.</param>
        /// <returns>A string containing the grid layout.</returns>
        public string DrawGrid(Position roverPosition)
        {
            var sb = new StringBuilder();

            // Top border
            sb.AppendLine(string.Concat(Enumerable.Repeat("🟦", Width + 2)));

            for (int row = 0; row < Height; row++)
            {
                // Left border
                sb.Append("🟦");
                // Determine what to display: rover, obstacle, or empty cell.
                for (int col = 0; col < Width; col++)
                    sb.Append(
                        roverPosition.X == col && roverPosition.Y == row ? "🚀" :
                        Obstacles.Any(o => o.X == col && o.Y == row) ? "🌑" : "🟫"
                    );
                // Right border
                sb.AppendLine("🟦"); 
            }

            // Bottom border
            sb.AppendLine(string.Concat(Enumerable.Repeat("🟦", Width + 2)));
            return sb.ToString();
        }
    }
}
