using System;

namespace Notidar.Sensole
{
    public interface ISensor
    {
        string Report(int sensorIndex, TimeSpan? timeFromPreviousCall);
    }
}
