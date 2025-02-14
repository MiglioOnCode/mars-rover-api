namespace MarsRoverAPI.Models.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when the rover encounters an obstacle.
    /// </summary>
    public class ObstacleEncounteredException : Exception
    {
        public ObstacleEncounteredException()
        {

        }

        public ObstacleEncounteredException(string message) 
            : base(message)
        {
        }
    }
}
