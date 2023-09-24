using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GettingStarted
{
    using Contracts;

    public class BrewConsumer : IConsumer<IMakeBrew>
    {
        readonly ILogger<BrewConsumer> _logger;

        public BrewConsumer(ILogger<BrewConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IMakeBrew> context)
        {
            _logger.LogInformation("Make Brew {0}\n - Sugar: {0}, Milk: {0}", context.MessageId, context.Message.Sugar, context.Message.Milk);
            //context.Publish<IMakeBrewCompled>(context.Message, _ => _.CorrelationId = context.MessageId);
            //context.Send<IMakeBrewCompled>(context.Message);

            context.Publish<IMakeBrewCompled>(new {
                context.Message.Sugar,
                context.Message.Milk,
                SomeNewValue = "bob"
            },  _ => _.CorrelationId = context.MessageId);

            return Task.CompletedTask;
        }
    }    
}