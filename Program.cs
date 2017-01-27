using servicedesk.Common.Events;
using servicedesk.Common.Host;
using servicedesk.Services.Tickets.Shared.Events;

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
                .SubscribeToEvent<TicketCreated>(exchangeName: "servicedesk.Services.Tickets", routingKey: "ticket.created")
                .SubscribeToEvent<CreateTicketRejected>(exchangeName: "servicedesk.Services.Tickets", routingKey: "ticket.rejected")
                .Build()
                .Run();
        }
    }
}
