using Microsoft.Extensions.Logging;

namespace Notidar.Sensole.Logging
{
    public static class MeterBuilderExtensions
    {
        public static MeterBuilder WithLoggerSink(this MeterBuilder meterBuilder, ILogger logger)
        {
            return meterBuilder.WithSink(report => logger.LogInformation(report));
        }

        public static MeterBuilder WithLoggerSink(this MeterBuilder meterBuilder, ILogger logger, LogLevel logLevel)
        {
            return meterBuilder.WithSink(report => logger.Log(logLevel, report));
        }
    }
}
