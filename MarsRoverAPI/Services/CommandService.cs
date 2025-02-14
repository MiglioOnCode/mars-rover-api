namespace MarsRoverAPI.Services
{
    /// <summary>
    /// Provides utility methods for processing rover commands.
    /// </summary>
    public class CommandService
    {
        private static readonly char[] commandsList = ['f', 'b', 'l', 'r'];

        /// <summary>
        /// Validates that all characters in the provided array are valid commands.
        /// Valid commands are: 'f' (forward), 'b' (backward), 'l' (left), and 'r' (right).
        /// </summary>
        /// <param name="commands">An array of command characters to validate.</param>
        /// <returns>
        /// <c>true</c> if all commands are valid; otherwise, <c>false</c>.
        /// </returns>
        public bool ValidateCommands(char[] commands)
        {
            return commands.All(c => commandsList.Contains(char.ToLower(c)));
        }
    }
}
