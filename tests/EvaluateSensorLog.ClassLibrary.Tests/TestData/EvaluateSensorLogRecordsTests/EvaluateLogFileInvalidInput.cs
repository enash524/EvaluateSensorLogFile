﻿using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData.EvaluateSensorLogRecordsTests
{
    public class EvaluateLogFileInvalidInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullWhitespaceString} (Parameter 'logContentsStr')" };
            yield return new object[] { string.Empty, $"{Messages.NullWhitespaceString} (Parameter 'logContentsStr')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}