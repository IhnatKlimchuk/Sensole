using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Notidar.Sensole
{
    public class Meter : IMeter
    {
        private readonly Task _task;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly ICollection<ISensor> _sensors;
        private readonly TimeSpan _reportPeriod;
        private readonly Action<string> _reportAction;
        private readonly Func<ReportContext, string>? _headerGenerator;
        private readonly Func<ReportContext, string>? _footerGenerator;

        internal Meter(
            IEnumerable<ISensor> sensors, 
            TimeSpan reportPeriod, 
            Action<string> reportAction,
            Func<ReportContext, string>? headerGenerator = null,
            Func<ReportContext, string>? footerGenerator = null)
        {
            _sensors = sensors.ToList();
            _reportPeriod = reportPeriod;
            _reportAction = reportAction;
            _headerGenerator = headerGenerator;
            _footerGenerator = footerGenerator;

            _cancellationTokenSource = new CancellationTokenSource();
            _task = Task.Run(ProcessAsync, _cancellationTokenSource.Token);
        }

        private async Task ProcessAsync()
        {
            DateTime initialTimestamp = DateTime.UtcNow;
            DateTime? previousRunTimestamp = null;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var currentRunTimestamp = DateTime.UtcNow;
                Report(initialTimestamp, previousRunTimestamp, currentRunTimestamp);
                previousRunTimestamp = currentRunTimestamp;
                try
                {
                    await Task.Delay(_reportPeriod, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // ignore
                }
            }

            Report(initialTimestamp, previousRunTimestamp, DateTime.UtcNow);
        }

        private void Report(DateTime initialTimestamp, DateTime? lastRunTimestamp, DateTime currentRunTimestamp)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                var context = new SensorReportContext
                {
                    Delta = lastRunTimestamp.HasValue ? currentRunTimestamp.Subtract(lastRunTimestamp.Value) : null,
                    InitialTimestamp = initialTimestamp,
                    ReportTimestamp = currentRunTimestamp,
                    SensorsCount = _sensors.Count
                };

                if (_headerGenerator != null)
                {
                    stringBuilder.AppendLine(_headerGenerator(context));
                }

                int sensorIndex = 0;
                foreach (var sensor in _sensors)
                {
                    stringBuilder.AppendLine(sensor.Report(context with { Index = sensorIndex }));
                    ++sensorIndex;
                }

                if (_footerGenerator != null)
                {
                    stringBuilder.AppendLine(_footerGenerator(context));
                }

                _reportAction(stringBuilder.ToString());
            }
            catch (Exception e)
            {
                _reportAction(e.ToString());
            }
        }

        public static MeterBuilder Builder => new();

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _task?.Wait();

            _cancellationTokenSource?.Dispose();
            _task?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
