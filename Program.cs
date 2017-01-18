using servicedesk.Common.Events;
using servicedesk.Common.Host;

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
                .SubscribeToEvent<NextStatusSet>(exchangeName: "servicedesk.statusmanagementsystem.events", routingKey: "nextstatusset")
                .SubscribeToEvent<SetNewStatusRejected>(exchangeName: "servicedesk.statusmanagementsystem.events", routingKey: "setnewstatusrejected")
                .Build()
                .Run();
        }
    }
}
