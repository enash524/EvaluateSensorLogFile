using System.Collections;
using System.Collections.Generic;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData
{
    public class HandleDecimalReadingInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "0", $"{Messages.NullWhitespaceString} (Parameter 'timestampInputValue')" };
            yield return new object[] { string.Empty, "0", $"{Messages.NullWhitespaceString} (Parameter 'timestampInputValue')" };
            yield return new object[] { "2019-02-29T22:00", "0", $"{Messages.InvalidDateTimeValue} (Parameter 'timestampInputValue')" };
            yield return new object[] { "2007-04-05T22:00", null, $"{Messages.NullWhitespaceString} (Parameter 'decimalInputValue')" };
            yield return new object[] { "2007-04-05T22:00", string.Empty, $"{Messages.NullWhitespaceString} (Parameter 'decimalInputValue')" };
            yield return new object[] { "2007-04-05T22:00", "a", $"{Messages.InvalidDecimalValue} (Parameter 'decimalInputValue')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}