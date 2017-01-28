using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.SignalR;
using servicedesk.SignalR.Hubs;
using servicedesk.Common.Events;
using Microsoft.Extensions.Logging;

namespace servicedesk.SignalR.Services
{
    public interface IBaseSignalRService
    {
        Task PublishForAllClientsAsync(IEvent @event);
        Task PublishForOneClientsAsync(IAuthenticatedEvent @event);
    }
    
    public class BaseSignalRService : IBaseSignalRService
    {
        private readonly ILogger logger;
        private readonly IHubContext<ServiceDeskHub> hub;

        public BaseSignalRService(IHubContext<ServiceDeskHub> hubContext, ILogger<BaseSignalRService> logger)
        {
            this.logger = logger;
            this.hub = hubContext;
        }

        public async Task PublishForAllClientsAsync(IEvent @event)
        {
            await hub.Clients.All.InvokeAsync(GetEventName(@event), @event);
            
            logger.LogDebug($"PublishForAllClients {GetEventName(@event)}: {ToJson(@event)}");
        }
        public async Task PublishForOneClientsAsync(IAuthenticatedEvent @event)
        {
            await hub.Clients.Client(@event.UserId).InvokeAsync(GetEventName(@event), @event);

            logger.LogDebug($"PublishForOneClients {GetEventName(@event)}: {ToJson(@event)}");
        }

        private string GetEventName(IEvent @event) 
            => @event.GetType().Name.Humanize(LetterCasing.LowerCase).Underscore();
        private string ToJson(IEvent @event) 
            => Newtonsoft.Json.JsonConvert.SerializeObject(@event);
    }
}