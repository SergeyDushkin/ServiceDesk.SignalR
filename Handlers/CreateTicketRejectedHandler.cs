using System.Threading.Tasks;
using servicedesk.Common.Events;
using servicedesk.Common.Services;
using servicedesk.Services.Tickets.Shared.Events;
using servicedesk.SignalR.Services;

namespace servicedesk.SignalR.Handlers
{
    public class CreateTicketRejectedHandler : IEventHandler<CreateTicketRejected>
    {
        private readonly IHandler handler;
        private readonly IBaseSignalRService service;

        public CreateTicketRejectedHandler(IHandler handler, IBaseSignalRService service)
        {
            this.handler = handler;
            this.service = service;
        }

        public async Task HandleAsync(CreateTicketRejected @event)
        {
            await handler
                .Run(async () => await service.PublishForAllClientsAsync(@event))
                .OnError((ex, logger) => logger.Error(ex, "Error during PublishForAllClientsAsync"))
                .ExecuteAsync();
        }
    }
}