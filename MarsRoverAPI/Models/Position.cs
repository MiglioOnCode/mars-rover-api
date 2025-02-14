namespace MarsRoverAPI.Models
{
    /// <summary>
    /// Represents a coordinate position on the grid.
    /// </summary>
    public class Position(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }
}
