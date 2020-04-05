namespace EvaluateSensorLog.ClassLibrary
{
    /// <summary>
    /// Defines the methods for parsing and validating sensor reading records
    /// </summary>
    public interface IEvaluateSensorLogRecords
    {
        /// <summary>
        /// Reads sensor log input text file and outputs sensor log report
        /// </summary>
        /// <param name="logContentsStr">The sensor log input text file</param>
        /// <exception cref="ArgumentException">logContentsStr is null, empty or whitespace</exception>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        string EvaluateLogFile(string logContentsStr);
    }
}