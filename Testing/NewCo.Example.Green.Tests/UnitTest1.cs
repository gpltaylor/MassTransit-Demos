using Contracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NewCo.Example.Green.Consumers.Consumers;

namespace NewCo.Example.Green.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        //
    }

    [Test]
    public async Task Test1()
    {
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<SetColourConsumer>();
            })
            .BuildServiceProvider(true);
        
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        await harness.Bus.Publish(new Contracts.SetColour { Colour = "Hello" });
        
        var consumerHarness = harness.GetConsumerHarness<SetColourConsumer>();
        Assert.That(await consumerHarness.Consumed.Any<Contracts.SetColour>());
    }

    [Test]
    public async Task Test2()
    {
        await using var provider = new ServiceCollection()
        .AddMassTransitTestHarness(x =>
        {
            x.AddConsumer<ColourCheckerConsumer>();
        })
        .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        var client = harness.GetRequestClient<ColourChecker>();
        var colour = client.GetResponse<ColourChecker>(new () { Colour = "Green" });

        Assert.AreEqual("Green", colour.Result.Message.Colour);
        Assert.IsTrue(colour.Result.Message.IsValid);
    }
}