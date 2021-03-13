using System;

namespace Notidar.Sensole
{
    public static class RequestSensorExtensions
    {
        public static MeterBuilder SenseRequests(this MeterBuilder meterBuilder, Func<long> valueAccessor, string name = default)
        {
            meterBuilder.Sense(new RequestSensor(valueAccessor, name));
            return meterBuilder;
        }
    }
}
