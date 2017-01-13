using System.Threading.Tasks;
using Coolector.Common.Events;
using Humanizer;
using Microsoft.AspNetCore.SignalR;
using servicedesk.Common.Events;
using servicedesk.SignalR.Hubs;
using NLog;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace servicedesk.SignalR.Services
{
    public class StatusSignalRService : IStatusSignalRService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHubContext<ServiceDeskHub> _hubContext;
        private readonly IHubContext _context;

        public StatusSignalRService(IHubContext<ServiceDeskHub> hubContext, IConnectionManager connectionManager)
        {
            _hubContext = hubContext;
            _context = connectionManager.GetHubContext<ServiceDeskHub>();
        }

        public async Task PublishStatusSetAsync(NextStatusSet @event)
        {
            var message = new
            {
                id = @event.RequestId,
                sourceId = @event.SourceId,
                referenceId = @event.ReferenceId,
                statusId = @event.StatusId
            };

            await _hubContext.Clients.All.InvokeAsync(GetEventName(@event), message);
            await _context.Clients.All.nextstatusset(message);

            Logger.Debug($"Publish: {GetEventName(@event)}");
        }

        public async Task PublishStatusRejectedAsync(SetNewStatusRejected @event)
        {
            var message = new
            {
                id = @event.RequestId,
                code = @event.Code,
                reason = @event.Reason,
                userId = @event.UserId
            };
            
            await _hubContext.Clients.All.InvokeAsync(GetEventName(@event), message);
            await _context.Clients.All.setnewstatusrejected(message);

            Logger.Debug($"Publish: {GetEventName(@event)}");
        }

        private string GetEventName(IEvent @event) 
            => @event.GetType().Name.Humanize(LetterCasing.LowerCase).Underscore();
    }
}