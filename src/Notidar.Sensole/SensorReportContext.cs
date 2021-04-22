namespace Notidar.Sensole
{
    public record SensorReportContext : ReportContext
    {
        public int Index { get; internal set; }
    }
}
