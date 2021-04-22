using System;
using System.Linq;

namespace Notidar.Sensole
{
    public static class MeterBuilderExtensions
    {
        public static MeterBuilder WithConsoleSink(this MeterBuilder meterBuilder)
        {
            return meterBuilder.WithSink(Console.WriteLine);
        }

        public static MeterBuilder WithConsoleSink(this MeterBuilder meterBuilder, bool replace = true)
        {
            if (replace)
            {
                int prevLines = 0;
                int prevLinePosition = 0;
                return meterBuilder.WithSink(report =>
                {
                    if (prevLinePosition == (Console.CursorTop - prevLines))
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - prevLines);
                    }
                    Console.Write(report);

                    prevLines = report.Count(x => x == '\r');
                    prevLinePosition = Console.CursorTop - prevLines;
                });
            }
            else
            {
                return meterBuilder.WithConsoleSink();
            }
        }

        public static MeterBuilder SenseOperation(this MeterBuilder meterBuilder, Func<long> valueAccessor, string? name = default)
        {
            return meterBuilder.Sense(new OperationSensor(valueAccessor, name));
        }

        public static MeterBuilder Sense(this MeterBuilder meterBuilder, Func<SensorReportContext, string> reportFunction)
        {
            return meterBuilder.Sense(new GenericSensor(reportFunction));
        }
    }
}
