namespace NewCo.Example.Green.Consumers.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class ColourCheckerConsumer :
        IConsumer<ColourChecker>
    {
        public Task Consume(ConsumeContext<ColourChecker> context)
        {

            context.RespondAsync(new ColourChecker { 
                CorrelationId = context.CorrelationId, 
                Colour = "Green",
                IsValid =  context.Message.Colour == "Green"
            });

            return Task.CompletedTask;
        }
    }
}