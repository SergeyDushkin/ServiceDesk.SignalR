using System.Threading.Tasks;
using servicedesk.Common.Events;
using servicedesk.Common.Services;
using servicedesk.SignalR.Services;

namespace servicedesk.SignalR.Handlers
{
    public class NextStatusSetHandler : IEventHandler<NextStatusSet>
    {
        private readonly IHandler _handler;
        private readonly IStatusSignalRService _signalRService;

        public NextStatusSetHandler(IHandler handler, IStatusSignalRService signalRService)
        {
            _handler = handler;
            _signalRService = signalRService;
        }

        public async Task HandleAsync(NextStatusSet @event)
        {
            await _handler
                .Run(async () => await _signalRService.PublishStatusSetAsync(@event))
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, "Error during PublishStatusSetAsync");
                })
                .ExecuteAsync();
        }
    }
}