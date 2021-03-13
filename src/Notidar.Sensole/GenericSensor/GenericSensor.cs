using System;

namespace Notidar.Sensole
{
    public class GenericSensor : ISensor
    {
        protected Func<int, TimeSpan?, string> _reportFunction;
        public GenericSensor(Func<int, TimeSpan?, string> reportFunction)
        {
            _reportFunction = reportFunction ?? throw new ArgumentNullException(nameof(reportFunction));
        }

        public string Report(int sensorIndex, TimeSpan? timeFromPreviousCall) => _reportFunction(sensorIndex, timeFromPreviousCall);
    }
}
