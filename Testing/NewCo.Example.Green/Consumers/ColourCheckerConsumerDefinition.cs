namespace NewCo.Example.Green.Consumers.Consumers
{
    using MassTransit;

    public class ColourCheckerConsumerDefinition :
        ConsumerDefinition<ColourCheckerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ColourCheckerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}