using System.ComponentModel;

namespace EvaluateSensorLog.Domain.Models
{
    public enum MonoxideStatus
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Keep")]
        Keep = 1,

        [Description("Discard")]
        Discard = 2
    }
}