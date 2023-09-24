namespace mtTopshelfAppConsumer
{
    using System;
    using System.Threading;
    using MassTransit;
    using EventContracts;
    using System.Threading.Tasks;

    public class consumerService 
    {
        public IBusControl BusControl { get; private set; }

        public async void Start()
        {
            BusControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("EventContracts:" + nameof(ValueEntered), e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await BusControl.StartAsync(source.Token);
        }

        public async void Stop() => await BusControl.StopAsync();
    }

    public class EventConsumer : IConsumer<ValueEntered>
    {
        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            await Task.Run( () => Console.WriteLine("Value: {0}", context.Message.Value) );
        }
    }
}