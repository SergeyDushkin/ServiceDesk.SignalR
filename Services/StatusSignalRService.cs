using System.Threading.Tasks;
using Coolector.Common.Events;
using Humanizer;
using Microsoft.AspNetCore.SignalR;
using servicedesk.Common.Events;
using servicedesk.SignalR.Hubs;

namespace servicedesk.SignalR.Services
{
    public class StatusSignalRService : IStatusSignalRService
    {
        private readonly IHubContext<ServiceDeskHub> _hubContext;

        public StatusSignalRService(IHubContext<ServiceDeskHub> hubContext)
        {
            _hubContext = hubContext;
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
        }

        private string GetEventName(IEvent @event) 
            => @event.GetType().Name.Humanize(LetterCasing.LowerCase).Underscore();
    }
}