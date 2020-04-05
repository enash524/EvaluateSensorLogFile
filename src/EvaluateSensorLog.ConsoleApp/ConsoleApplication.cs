using System;
using System.IO;
using System.Threading.Tasks;
using EvaluateSensorLog.ClassLibrary;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.ConsoleApp
{
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
        public async Task Run()
        {
            try
            {
                string input = await File.ReadAllTextAsync("input.txt");
                string output = _evaluateSensorLogRecords.EvaluateLogFile(input);

                Console.WriteLine("Devices and classification:");
                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
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
