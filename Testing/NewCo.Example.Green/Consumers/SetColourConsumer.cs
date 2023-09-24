namespace NewCo.Example.Green.Consumers.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;


    public class SetColourConsumer :
        IConsumer<SetColour>
    {
        private ILogger<SetColourConsumer> _logger;


        public SetColourConsumer(ILogger<SetColourConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SetColour> context)
        {
            _logger.LogInformation($"SetColourConsumer: {context.Message.Colour}");
            return Task.CompletedTask;
        }
    }
}