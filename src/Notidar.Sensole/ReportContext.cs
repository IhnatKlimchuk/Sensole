using System;

namespace Notidar.Sensole
{
    public record ReportContext
    {
        public int SensorsCount { get; internal set; }
        public DateTime InitialTimestamp { get; internal set; }
        public DateTime ReportTimestamp { get; internal set; }
        public TimeSpan? Delta { get; internal set; }
    }
}
