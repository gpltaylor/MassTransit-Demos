using System;

namespace mtRequest
{
    using MassTransit;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading;
    using System.Threading.Tasks;

    public interface CheckOrderStatus
    {
        string OrderId { get; }
    }

    public interface OrderStatusResult
    {
        string OrderId { get; }
        DateTime Timestamp { get; }
        short StatusCode { get; }
        string StatusText { get; }
    }

    class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddMassTransit(x => {
                x.UsingRabbitMq((context,cfg) => {
                    
                    cfg.ReceiveEndpoint("mtRequest:CheckOrderStatus", e => {
                        e.Consumer<CheckOrderStatusConsumer>();
                    });
                });

                x.AddRequestClient<CheckOrderStatus>();
            });


            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // DI prividers
            var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IRequestClient<CheckOrderStatus>>();
            var busControl = provider.GetRequiredService<IBusControl>();

            await busControl.StartAsync(source.Token);

            var responseA = client.GetResponse<OrderStatusResult>(new {OrderId = InVar.Id});
            var responseB = client.GetResponse<OrderStatusResult>(new {OrderId = InVar.Id});
            var responseC = client.GetResponse<OrderStatusResult>(new {OrderId = InVar.Id});

            await Task.WhenAll(responseA, responseB, responseC);

            Console.WriteLine("Response A: " + responseA.Result.MessageId);
            Console.WriteLine("Response B: " + responseB.Result.MessageId);
            Console.WriteLine("Response C: " + responseC.Result.MessageId);

            try
            {
                while (true)
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }

    public class CheckOrderStatusConsumer : IConsumer<CheckOrderStatus>
    {
        public async Task Consume(ConsumeContext<CheckOrderStatus> context)
        {
            Console.WriteLine("Calling: CheckOrderStatusConsumer.Consume");
            Thread.Sleep(2000);

            await context.RespondAsync<OrderStatusResult>(new 
            {
                OrderId = InVar.Id,
                Timestamp = InVar.Timestamp,
                StatusCode = 1,
                StatusText = "New app v2"
            });
        }
    }
}
