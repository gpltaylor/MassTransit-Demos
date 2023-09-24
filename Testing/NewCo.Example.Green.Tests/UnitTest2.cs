using Contracts;
using MassTransit;
using MassTransit.TestFramework;
using Microsoft.Extensions.DependencyInjection;
using NewCo.Example.Green.Consumers.Consumers;

namespace NewCo.Example.Green.Tests;

public class Tests2 : InMemoryTestFixture
{
    private ServiceProvider _provider;
    private Task<ConsumeContext<SetColour>> _received;

    public Tests2()
    {
        //
        var services = new ServiceCollection();
        services.AddScoped<SetColourConsumer>();
        
        services.AddMassTransitTestHarness(cfg =>
        {
            cfg.AddConsumer<SetColourConsumer>();
        });

        _provider = services.BuildServiceProvider();  
    }

    [Test]
    public async Task Test1()
    {
        var mycolour = new Contracts.SetColour() { Colour = "Green" };
        // await InputQueueSendEndpoint.Send(mycolour);
        await Bus.Publish<Contracts.SetColour>(mycolour);
        await _received;
    }

    protected override void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
    {
        configurator.Consumer<SetColourConsumer>(_provider);
        _received = Handled<Contracts.SetColour>(configurator);
        //
    }

}