using System;
using System.Threading;

namespace Notidar.Sensole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            long counterValue = 0;
            using var meter = Meter.Builder
                .SenseRequests(() => counterValue, name: "Requests total")
                .Sense(new RequestSensor(() => counterValue / 2, "Requests success"))
                .WithSink(report =>
                {
                    Console.SetCursorPosition(0,0);
                    Console.Write(report);
                })
                .Each(TimeSpan.FromSeconds(1));


            while (true)
            {
                Interlocked.Increment(ref counterValue);
            }
        }
    }
}
