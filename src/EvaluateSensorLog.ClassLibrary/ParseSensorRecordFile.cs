using System;
using System.Collections.Generic;
using System.Text;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.ClassLibrary
{
    public class ParseSensorRecordFile : IParseSensorRecordFile
    {
        private readonly ILogger<ParseSensorRecordFile> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordFile`1"/> class.
        /// </summary>
        /// <param name="logger">DI injected logger</param>
        public ParseSensorRecordFile(ILogger<ParseSensorRecordFile> logger)
        {
            _logger = logger;
        }
    }
}
