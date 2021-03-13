using System;
using System.Collections.Generic;

namespace Notidar.Sensole
{
    public class MeterBuilder
    {
        private List<ISensor> _senors = new List<ISensor>();
        private Action<string> _reportAction = null;
        private Func<TimeSpan, string> _headerGenerator = null;
        internal MeterBuilder()
        {

        }
        public MeterBuilder Sense(ISensor sensor)
        {
            _senors.Add(sensor);
            return this;
        }
        public MeterBuilder WithHeader(Func<TimeSpan, string> headerGenerator)
        {
            _headerGenerator = headerGenerator;
            return this;
        }
        public MeterBuilder WithHeader(Func<string> headerGenerator)
        {
            _headerGenerator = (timePeriod) => headerGenerator();
            return this;
        }
        public MeterBuilder WithSink(Action<string> reportAction)
        {
            _reportAction = reportAction;
            return this;
        }
        public IMeter Each(TimeSpan reportPeriod)
        {
            return new Meter(_senors, reportPeriod, _reportAction);
        }
    }
}
