using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GettingStarted
{
    using Contracts;

    public class BrewCompletedConsumer : IConsumer<IMakeBrewCompled>
    {
        readonly ILogger<BrewCompletedConsumer> _logger;

        public BrewCompletedConsumer(ILogger<BrewCompletedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IMakeBrewCompled> context)
        {
            _logger.LogInformation("Time to drink the Brew {0} - {0}", context.Message.SomeNewValue, context.CorrelationId);
            return Task.CompletedTask;
        }
    }


    public class WashingUpConsumer : IConsumer<IMakeBrewCompled>
    {
        readonly ILogger<WashingUpConsumer> _logger;

        public WashingUpConsumer(ILogger<WashingUpConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IMakeBrewCompled> context)
        {
            _logger.LogInformation("Get ready to washup Brew - {0}", context.CorrelationId);
            return Task.CompletedTask;
        }
    } 
}