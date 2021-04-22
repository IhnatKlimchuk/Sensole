using System;
using System.Threading;

namespace Notidar.Sensole.Sample
{
    class Program
    {
        static void Main()
        {
            long counterValue = 0;
            long successCounterValue = 0;
            Console.WriteLine("Start counting... ");

            using var meter = Meter.Builder
                .SenseOperation(() => counterValue, name: "Requests")
                .SenseOperation(() => successCounterValue, name: "Requests success")
                .SenseOperation(() => counterValue - successCounterValue, name: "Requests failed")
                .Sense(context => $"Report delay {context.Delta}")
                .WithHeader(context => $"Measuring for {DateTime.UtcNow.Subtract(context.InitialTimestamp)}...")
                .WithFooter(_ => "That's how we roll!")
                .WithConsoleSink(replace: true)
                .Each(TimeSpan.FromSeconds(0.1));

            var random = new Random();
            while (!Console.KeyAvailable)
            {
                Interlocked.Increment(ref counterValue);
                Interlocked.Add(ref successCounterValue, random.Next() % 2);
            }
        }
    }
}