using System;
using System.Threading.Tasks;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.ConsoleApp
{
    /// <summary>
    /// Main class to start the console application
    /// </summary>
    public class ConsoleApplication
    {
        private readonly IEvaluateSensorLogRecords _evaluateSensorLogRecords;
        private readonly ILogger<ConsoleApplication> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleApplication`1"/> class.
        /// </summary>
        /// <param name="evaluateSensorLogRecords"></param>
        /// <param name="logger">DI injected logger</param>
        public ConsoleApplication(IEvaluateSensorLogRecords evaluateSensorLogRecords, ILogger<ConsoleApplication> logger)
        {
            _evaluateSensorLogRecords = evaluateSensorLogRecords;
            _logger = logger;
        }

        /// <summary>
        /// Run the program
        /// </summary>
        /// <param name="path">The path to the sensor log input text file</param>
        public async Task Run(string path)
        {
            try
            {
                string output = await _evaluateSensorLogRecords.EvaluateLogFileAsync(path);

                Console.WriteLine("Devices and classification:");
                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Console.WriteLine($"Exception: {ex}");
            }
            finally
            {
                Console.WriteLine("--- Press Any Key To Continue ---");
                Console.ReadKey(true);
            }
        }
    }
}
