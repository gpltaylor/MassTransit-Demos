namespace NewCo.Example.Green.Consumers.Consumers
{
    using MassTransit;

    public class SetColourConsumerDefinition :
        ConsumerDefinition<SetColourConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SetColourConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}