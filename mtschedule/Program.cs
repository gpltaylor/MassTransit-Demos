using System;

namespace mtschedule
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        private static IBusControl busControl;

        public static async Task Main()
        {
            var services = new ServiceCollection();


            services.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                
                x.UsingRabbitMq((context, cfg) => 
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(context);

                    cfg.ReceiveEndpoint("mtschedule:SendNotification", e =>
                    {
                        e.Consumer<SendNotificationConsumer>();
                    });
                });

            });

            var provider = services.BuildServiceProvider();

            var scheduler = provider.GetRequiredService<IMessageScheduler>();

            await scheduler.SchedulePublish<SendNotification>(
                DateTime.UtcNow + TimeSpan.FromSeconds(30), new 
                {
                    EmailAddress = "frank@nul.org",
                    Body =  "Thank you for signing up for our awesome newsletter!"
                });

            try
            {
                busControl = provider.GetRequiredService<IBusControl>();
                await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await busControl.StopAsync();
                Environment.Exit(1);
            }

            while (true)
            {
                string value = await Task.Run(() =>
                {
                    Console.WriteLine("Enter Error code (or quit to exit)");
                    Console.Write("> ");
                    return Console.ReadLine();
                });
            }
        }
    }

    class SendNotificationConsumer : IConsumer<SendNotification>
    {
        public async Task Consume(ConsumeContext<SendNotification> context)
        {
            await Task.Run( () => {
                Console.WriteLine("email: {0} body: {1} delay: {2}",
                    context.Message.EmailAddress,
                    context.Message.Body,
                    context.ReceiveContext.TransportHeaders.Get<string>("x-delay"));
            });
        }
    }

    public class ScheduleNotificationConsumer : IConsumer<ScheduleNotification>
    {
        public async Task Consume(ConsumeContext<ScheduleNotification> context)
        {
            Uri notificationService = new Uri("queue:notification-service");

            await context.ScheduleSend<SendNotification>(notificationService,
                context.Message.DeliveryTime,
                new 
                {
                    EmailAddress = context.Message.EmailAddress,
                    Body =  context.Message.Body
                });
        }

        class SendNotificationCommand : SendNotification
        {
            public string EmailAddress { get; set; }
            public string Body { get; set; }
        }
    }

    public interface ScheduleNotification
    {
        DateTime DeliveryTime { get; }
        string EmailAddress { get; }
        string Body { get; }
    }

    public interface SendNotification
    {
        string EmailAddress { get; }
        string Body { get; }
    }
}
