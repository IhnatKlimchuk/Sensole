using System;
using System.Threading;

namespace Notidar.Sensole.Sample
{
    class Program
    {
        static void Main()
        {
            var random = new Random();

            long counterValue = 0;
            long successCounterValue = 0;
            Console.WriteLine("Start counting... ");

            using var meter = Meter.Builder
                    .SenseRequests(() => counterValue, name: "Requests")
                    .SenseRequests(() => successCounterValue, name: "Requests success")
                    .SenseRequests(() => counterValue - successCounterValue, name: "Requests failed")
                    .WithHeader(context => $"Measuring for {DateTime.UtcNow.Subtract(context.InitialTimestamp)}...")
                    .WithConsoleSink(replace: true)
                    .Each(TimeSpan.FromSeconds(0.1));

            while (!Console.KeyAvailable)
            {
                Interlocked.Increment(ref counterValue);
                Interlocked.Add(ref successCounterValue, random.Next() % 2);
            }
        }
    }
}
