using System;

namespace mtExceptions
{
    using System.Threading.Tasks;
    using EventContracts;
    using MassTransit;
    using GreenPipes;
    using System.Threading;

    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting mtExceptions");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("submit-order", e =>
                {
                    e.UseMessageRetry(r => {
                        //r.Immediate(5);
                        r.Interval(5, TimeSpan.FromSeconds(1));
                        r.Ignore<InvalidOperationException>( e=> e.Message == "tick tick boom");
                    });
                    // e.UseMessageRetry(r => r.Exponential(
                    //     10,
                    //     TimeSpan.FromMilliseconds(1000),
                    //     TimeSpan.FromMilliseconds(10000),
                    //     TimeSpan.FromMilliseconds(500)
                    // ));

                    e.Consumer<SubmitOrderConsumer>();

                    cfg.ReceiveEndpoint("submit-order_fault", e => {
                        e.Consumer<SubmitOrderFaultConsumer>();
                    });
                });
            });


            var endpoint = await busControl.GetSendEndpoint( new Uri("queue:submit-order"));
            await endpoint.Send<SubmitOrder>( new { OrderId = MassTransit.InVar.Id, ErrorId = 1 });

            await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

            while (true)
            {
                string value = await Task.Run(() =>
                {
                    Console.WriteLine("Enter Error code (or quit to exit)");
                    Console.Write("> ");
                    return Console.ReadLine();
                    busControl.Stop();
                });
            }
        }
    }

    public class SubmitOrderConsumer :
    IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            Console.WriteLine("Hello from Consumer");

            if(context.Message.ErrorId == 1) {
                throw new InvalidOperationException("boom");
            }

            await Task.Run(() => Console.WriteLine("Everything worked"));
            
        }
    }

    public class SubmitOrderFaultConsumer : IConsumer<Fault<SubmitOrder>>
    {
        public async Task Consume(ConsumeContext<Fault<SubmitOrder>> context)
        {
            await Task.Run(() => Console.WriteLine("boom something went wrong"));
        }
    }
}
