using System;

namespace Notidar.Sensole
{
    public class GenericSensor : ISensor
    {
        protected Func<SensorReportContext, string> _reportFunction;
        public GenericSensor(Func<SensorReportContext, string> reportFunction)
        {
            _reportFunction = reportFunction ?? throw new ArgumentNullException(nameof(reportFunction));
        }

        public string Report(SensorReportContext context) => _reportFunction(context);
    }
}
