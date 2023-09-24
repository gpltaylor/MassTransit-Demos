using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;

namespace GettingStarted
{
    using Contracts;

    public class Program
    {
        public static void Main(string[] args)
        {
            EndpointConvention.Map<IMakeBrew>(new Uri("queue:Brew"));
            EndpointConvention.Map<IMessage>(new Uri("queue:Message"));
            EndpointConvention.Map<Sample.Platform.SampleCommand>(new Uri("queue:sample"));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumersFromNamespaceContaining<BrewConsumer>();

                        x.UsingRabbitMq((context,cfg) =>
                        {
                           cfg.ConfigureEndpoints(context);
                        });
                        
                    });
                    
                    services.AddMassTransitHostedService(true);
                    
                    services.AddHostedService<Worker>();
                });
    }
}
