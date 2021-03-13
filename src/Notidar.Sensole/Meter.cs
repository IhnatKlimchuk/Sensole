using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notidar.Sensole
{
    public class Meter : IMeter
    {
        private Task _task;
        private CancellationTokenSource _cancellationTokenSource;
        private IEnumerable<ISensor> _sensors;
        private TimeSpan _reportPeriod;
        private Action<string> _reportAction;
        internal Meter(IEnumerable<ISensor> sensors, TimeSpan reportPeriod, Action<string> reportAction)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _sensors = sensors;
            _reportPeriod = reportPeriod;
            _reportAction = reportAction;
            _task = Task.Run(async () => 
            {
                DateTime? lastRunTimestamp = null;
                var stringBuilder = new StringBuilder();
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        var currentRunTimestamp = DateTime.UtcNow;
                        TimeSpan? currentRunPeriod = lastRunTimestamp.HasValue ? currentRunTimestamp.Subtract(lastRunTimestamp.Value) : (TimeSpan?)null;
                        stringBuilder.Clear();
                        foreach (var sensor in _sensors)
                        {
                            stringBuilder.AppendLine(sensor.Report(0, currentRunPeriod));
                        }
                        _reportAction(stringBuilder.ToString());
                        lastRunTimestamp = currentRunTimestamp;
                        await Task.Delay(_reportPeriod, _cancellationTokenSource.Token);
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public static MeterBuilder Builder => new MeterBuilder();

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _task?.Wait();

            _cancellationTokenSource?.Dispose();
            _task?.Dispose();

            _cancellationTokenSource = null;
            _task = null;

            GC.SuppressFinalize(this);
        }
    }
}
