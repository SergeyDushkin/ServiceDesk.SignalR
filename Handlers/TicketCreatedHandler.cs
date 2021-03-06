using System.Threading.Tasks;
using servicedesk.Common.Events;
using servicedesk.Common.Services;
using servicedesk.Services.Tickets.Shared.Events;
using servicedesk.SignalR.Services;

namespace servicedesk.SignalR.Handlers
{
    public class TicketCreatedHandler : IEventHandler<TicketCreated>
    {
        private readonly IHandler handler;
        private readonly IBaseSignalRService service;

        public TicketCreatedHandler(IHandler handler, IBaseSignalRService service)
        {
            this.handler = handler;
            this.service = service;
        }

        public async Task HandleAsync(TicketCreated @event)
        {
            await handler
                .Run(async () => await service.PublishForAllClientsAsync(@event))
                .OnSuccess((logger) => logger.Debug("PublishForAllClientsAsync successed"))
                .OnError((ex, logger) => logger.Error(ex, "Error during PublishForAllClientsAsync"))
                .ExecuteAsync();
        }
    }
}