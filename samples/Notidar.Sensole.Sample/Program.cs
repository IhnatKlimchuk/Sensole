using System;
using System.Threading;

namespace Notidar.Sensole.Sample
{
    class Program
    {
        static void Main()
        {
            long counterValue = 0;
            Console.WriteLine("Start counting... ");

            {
                using var meter = Meter.Builder
                    .SenseRequests(() => counterValue, name: "Requests total")
                    .WithHeader(context => $"Measuring for {DateTime.UtcNow.Subtract(context.InitialTimestamp)}...")
                    .WithConsoleSink(replace: true)
                    .Each(TimeSpan.FromSeconds(0.5));

                while (!Console.KeyAvailable)
                {
                    Interlocked.Increment(ref counterValue);
                    if (counterValue % 1000000 == 0)
                    {
                        Console.WriteLine("Blah... ");
                    }
                }
            }
        }
    }
}
