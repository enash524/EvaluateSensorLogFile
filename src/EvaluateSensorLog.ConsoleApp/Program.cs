using System;
using System.IO;
using System.Threading.Tasks;
using EvaluateSensorLog.ClassLibrary;

namespace EvaluateSensorLog.ConsoleApp
{
    /// <summary>
    /// Starts the console application
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main entry point to ConsoleApp
        /// </summary>
        /// <param name="args">Collection of command line arguments. Currently unused.</param>
        private static async Task Main(string[] args)
        {
            IEvaluateSensorLogRecords evaluateSensorLogRecords = new EvaluateSensorLogRecords();

            try
            {
                string input = await File.ReadAllTextAsync("input.txt");
                string output = evaluateSensorLogRecords.EvaluateLogFile(input);

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