using System;

namespace Notidar.Sensole
{
    public static class GenericExtensions
    {
        public static MeterBuilder Sense(this MeterBuilder meterBuilder, Func<int, TimeSpan?, string> reportFunction)
        {
            return meterBuilder.Sense(new GenericSensor(reportFunction));
        }

        public static MeterBuilder Sense(this MeterBuilder meterBuilder, Func<TimeSpan?, string> reportFunction)
        {
            return meterBuilder.Sense(new GenericSensor((_, timeFromPreviousCall) => reportFunction(timeFromPreviousCall)));
        }

        public static MeterBuilder Sense(this MeterBuilder meterBuilder, Func<int, string> reportFunction)
        {
            return meterBuilder.Sense(new GenericSensor((sensorIndex, _) => reportFunction(sensorIndex)));
        }

        public static MeterBuilder Sense(this MeterBuilder meterBuilder, Func<string> reportFunction)
        {
            return meterBuilder.Sense(new GenericSensor((_, _) => reportFunction()));
        }
    }
}
