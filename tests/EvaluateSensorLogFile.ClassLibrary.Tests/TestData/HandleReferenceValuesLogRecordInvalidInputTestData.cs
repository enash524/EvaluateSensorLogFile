using System.Collections;
using System.Collections.Generic;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class HandleReferenceValuesLogRecordInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "0", "0", $"{Messages.NullWhitespaceString} (Parameter 'thermometerInputValue')" };
            yield return new object[] { string.Empty, "0", "0", $"{Messages.NullWhitespaceString} (Parameter 'thermometerInputValue')" };
            yield return new object[] { "a", "0", "0", $"{Messages.InvalidDecimalValue} (Parameter 'thermometerInputValue')" };
            yield return new object[] { "0", null, "0", $"{Messages.NullWhitespaceString} (Parameter 'humidityInputValue')" };
            yield return new object[] { "0", string.Empty, "0", $"{Messages.NullWhitespaceString} (Parameter 'humidityInputValue')" };
            yield return new object[] { "0", "a", "0", $"{Messages.InvalidDecimalValue} (Parameter 'humidityInputValue')" };
            yield return new object[] { "0", "0", null, $"{Messages.NullWhitespaceString} (Parameter 'monoxideInputValue')" };
            yield return new object[] { "0", "0", string.Empty, $"{Messages.NullWhitespaceString} (Parameter 'monoxideInputValue')" };
            yield return new object[] { "0", "0", "a", $"{Messages.InvalidIntegerValue} (Parameter 'monoxideInputValue')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}