using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GettingStarted
{
    using Contracts;
    
    public class Testimonial
    {
        public string Text { get; set; }
    }

    public class MessageConsumer :
        IConsumer<IMessage>
    {
        readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IMessage> context)
        {
            _logger.LogInformation("MessageConsumer - Received Text: {Text}", context.Message.Text);

            return Task.CompletedTask;
        }
    }
}