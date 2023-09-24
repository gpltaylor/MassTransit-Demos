using System;

namespace mtTopshelfAppConsumer
{
    using Topshelf;

    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run( x => {
                x.Service<consumerService>(s=> {
                    s.ConstructUsing(name => new consumerService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalService();
                x.SetDescription("RabbitMQ - ValueEntered Consumer");
            });
        }
    }
}
