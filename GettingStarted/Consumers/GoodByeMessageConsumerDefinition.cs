namespace GettingStarted.Consumers
{
    using MassTransit;
    using MassTransit.ConsumeConfigurators;
    using MassTransit.Definition;
    using GreenPipes;

    public class GoodByeMessageConsumerDefinition :
        ConsumerDefinition<GoodByeMessageConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GoodByeMessageConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000, 2000));
        }
    }
}