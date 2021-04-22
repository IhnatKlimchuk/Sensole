using System;

namespace Notidar.Sensole
{
    public class OperationSensor : ISensor
    {
        private readonly Func<long> _valueAccessor = () => default;
        private readonly string? _name = null;
        private long _currentValue = default;

        public OperationSensor(Func<long> valueAccessor, string? name = default, long currentValue = default)
        {
            _valueAccessor = valueAccessor;
            _name = name;
            _currentValue = currentValue;
        }

        public string Report(SensorReportContext context)
        {
            long previousValue = _currentValue;
            _currentValue = _valueAccessor();
            return $"{(string.IsNullOrEmpty(_name) ? $"Sensor {context.Index}" : _name)}: total value {_currentValue}, frequency {(context.Delta.HasValue ? ((_currentValue - previousValue) / context.Delta.Value.TotalSeconds) : 0):N2} ops";
        }
    }
}
