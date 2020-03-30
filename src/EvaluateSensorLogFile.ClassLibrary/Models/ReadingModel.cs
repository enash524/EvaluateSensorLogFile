﻿using System;

namespace EvaluateSensorLogFile.ClassLibrary.Models
{
    /// <summary>
    /// Represents a sensor reading record
    /// </summary>
    /// <typeparam name="T">Type of the reading value</typeparam>
    public class ReadingModel<T>
    {
        /// <summary>
        /// Time stamp of the reading
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Sensor reading value
        /// </summary>
        public T Value { get; set; }
    }
}