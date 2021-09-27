namespace EvaluateSensorLog.Application.Models
{
    /// <summary>
    /// Represents the result and result type of a command.
    /// </summary>
    /// <typeparam name="T">The type of the result object.</typeparam>
    public class CommandResult<T>
    {
        /// <summary>
        /// The result of the command
        /// </summary>
        public CommandResultType CommandResultType { get; set; }

        /// <summary>
        /// The returned result object
        /// </summary>
        public T Result { get; set; }
    }
}
