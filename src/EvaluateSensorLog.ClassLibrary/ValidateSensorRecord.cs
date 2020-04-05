using System;
using System.Collections.Generic;
using System.Text;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.ClassLibrary
{
    public class ValidateSensorRecord : IValidateSensorRecord
    {
        private readonly ILogger<ValidateSensorRecord> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSensorRecord`1"/> class.
        /// </summary>
        /// <param name="logger">DI injected logger</param>
        public ValidateSensorRecord(ILogger<ValidateSensorRecord> logger)
        {
            _logger = logger;
        }
    }
}
