using System;

namespace Notidar.Sensole
{
    public class RequestSensor : ISensor
    {
        private Func<long> _valueAccessor = () => default;
        private string _name = null;
        private long _currentValue = default;

        public RequestSensor(Func<long> valueAccessor, string name = default, long currentValue = default)
        {
            _valueAccessor = valueAccessor;
            _name = name;
            _currentValue = currentValue;
        }

        public string Report(int sensorIndex, TimeSpan? timeFromPreviousCall)
        {
            long previousValue = _currentValue;
            _currentValue = _valueAccessor();

            return $"{(string.IsNullOrEmpty(_name) ? $"Sensor {sensorIndex}" : _name)}: total value {_currentValue}, frequency {(timeFromPreviousCall.HasValue ? ((_currentValue - previousValue) / timeFromPreviousCall.Value.TotalSeconds) : 0):N2} rps";
        }
    }
}
