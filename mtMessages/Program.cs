namespace mtMessages
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventContracts;
    using MassTransit;
    using MassTransit.Topology.Topologies;

    class Program
    {
        static async Task Main(string[] args)
        {
            GlobalTopology.Send.UseCorrelationId<SubmitOrder>(x => x.OrderId);

            var busControl = Bus.Factory.CreateUsingRabbitMq();

            await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

            try
            {
                var endpoint = await busControl.GetSendEndpoint(new Uri("queue:order-service"));
                var endpointSamplecommand = await busControl.GetSendEndpoint(new Uri("queue:sample-command"));

                // Set CorrelationId using SendContext<T>
                await endpoint.Send<SubmitOrder>(new { OrderId = InVar.Id });
                await busControl.Publish<SubmitOrder>(new { OrderID = InVar.Id });
                await endpointSamplecommand.Send<Sample.Platform.SampleCommand>(new { });

            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
