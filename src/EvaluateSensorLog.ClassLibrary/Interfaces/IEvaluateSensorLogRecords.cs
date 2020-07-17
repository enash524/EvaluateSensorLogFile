using System.Threading.Tasks;

namespace EvaluateSensorLog.ClassLibrary.Interfaces
{
    /// <summary>
    /// Defines the methods for parsing and validating sensor reading records
    /// </summary>
    public interface IEvaluateSensorLogRecords
    {
        /// <summary>
        /// Reads sensor log input text file and outputs sensor log report
        /// </summary>
        /// <param name="path">The path to the sensor log input text file</param>
        /// <exception cref="ArgumentException">logContentsStr is null, empty or whitespace</exception>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        Task<string> EvaluateLogFileAsync(string path);
    }
}