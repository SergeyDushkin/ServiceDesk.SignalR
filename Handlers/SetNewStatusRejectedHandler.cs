using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using servicedesk.Common.Events;
using servicedesk.SignalR.Services;

namespace servicedesk.SignalR.Handlers
{
    public class SetNewStatusRejectedHandler : IEventHandler<SetNewStatusRejected>
    {
        private readonly IHandler _handler;
        private readonly IStatusSignalRService _signalRService;

        public SetNewStatusRejectedHandler(IHandler handler, IStatusSignalRService signalRService)
        {
            _handler = handler;
            _signalRService = signalRService;
        }

        public async Task HandleAsync(SetNewStatusRejected @event)
        {
            await _handler
                .Run(async () => await _signalRService.PublishStatusRejectedAsync(@event))
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, "Error during PublishStatusRejectedAsync");
                })
                .ExecuteAsync();
        }
    }
}