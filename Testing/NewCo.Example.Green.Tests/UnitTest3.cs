using Contracts;
using MassTransit;
using MassTransit.TestFramework;
using NewCo.Example.Green.Consumers.Consumers;

namespace NewCo.Example.Green.Tests;

public class Tests3 : InMemoryTestFixture
{
    private IRequestClient<ColourChecker> _requestClient = null!;

    [SetUp]
    public void Setup()
    {
        _requestClient = CreateRequestClient<ColourChecker>();
    }

    [Test]
    public async Task Test3()
    {
        
       var colourChecker = new ColourChecker() { Colour = "Green" };
        
        var results = await _requestClient.GetResponse<ColourChecker>(colourChecker);
        Assert.AreEqual(results.Message.Colour, "Green");
        Assert.True(results.Message.IsValid);
    }

    protected override void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
    {
        configurator.Consumer<ColourCheckerConsumer>();
    }

}