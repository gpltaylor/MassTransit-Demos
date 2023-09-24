using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace GettingStarted
{
    using Contracts;

    public class Worker : BackgroundService
    {
        readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var d = DateTimeOffset.Now;
                Console.WriteLine("Publishing message {0}", d);
                
                await _bus.Send<IMessage>(new { 
                    Text = $"The time is {DateTimeOffset.Now}"
                });
                
                await _bus.Send<IMakeBrew>(new { 
                    Sugar = 0, 
                    Milk = true,
                });

                await _bus.Send<Sample.Platform.SampleCommand>(new {
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
