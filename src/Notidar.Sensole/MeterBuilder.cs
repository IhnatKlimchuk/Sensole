using System;
using System.Collections.Generic;

namespace Notidar.Sensole
{
    public class MeterBuilder
    {
        private readonly ICollection<ISensor> _senors;
        private Action<string> _reportAction;
        private Func<ReportContext, string>? _headerGenerator;
        private Func<ReportContext, string>? _footerGenerator;

        internal MeterBuilder()
        {
            _senors = new List<ISensor>();
            _reportAction = Console.WriteLine;
            _headerGenerator = null;
            _footerGenerator = null;
        }

        public MeterBuilder Sense(ISensor sensor)
        {
            _senors.Add(sensor);
            return this;
        }

        public MeterBuilder WithHeader(Func<ReportContext, string> headerGenerator)
        {
            _headerGenerator = headerGenerator;
            return this;
        }

        public MeterBuilder WithSink(Action<string> reportAction)
        {
            _reportAction = reportAction;
            return this;
        }

        public MeterBuilder WithFooter(Func<ReportContext, string> footerGenerator)
        {
            _footerGenerator = footerGenerator;
            return this;
        }

        public IMeter Each(TimeSpan reportPeriod)
        {
            return new Meter(_senors, reportPeriod, _reportAction, _headerGenerator, _footerGenerator);
        }
    }
}
