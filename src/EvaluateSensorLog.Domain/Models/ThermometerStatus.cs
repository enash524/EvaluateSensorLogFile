using System.ComponentModel;

namespace EvaluateSensorLog.Domain.Models
{
    public enum ThermometerStatus
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Ultra Precise")]
        UltraPrecise = 1,

        [Description("Very Precise")]
        VeryPrecise = 2,

        [Description("Precise")]
        Precise = 3
    }
}
