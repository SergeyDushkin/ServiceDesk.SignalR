using Coolector.Common.Host;
using servicedesk.Common.Events;

namespace servicedesk.SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 15000)
                .UseAutofac(Startup.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<NextStatusSet>()
                .SubscribeToEvent<SetNewStatusRejected>()
                .Build()
                .Run();
        }
    }
}
