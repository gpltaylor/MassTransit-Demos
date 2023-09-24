namespace GettingStarted.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class GoodByeMessageConsumer : IConsumer<GoodByeMessage>
    {
        public Task Consume(ConsumeContext<GoodByeMessage> context)
        {
            return Task.CompletedTask;
        }
    }
}