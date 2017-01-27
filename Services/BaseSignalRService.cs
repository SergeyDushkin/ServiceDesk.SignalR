using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.SignalR;
using servicedesk.SignalR.Hubs;
using NLog;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using servicedesk.Common.Events;

namespace servicedesk.SignalR.Services
{
    public interface IBaseSignalRService
    {
        Task PublishForAllClientsAsync(IEvent @event);
        Task PublishForOneClientsAsync(IAuthenticatedEvent @event);
    }
    
    public class BaseSignalRService : IBaseSignalRService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHubContext<ServiceDeskHub> _hubContext;

        //private readonly IHubContext _context;

        public BaseSignalRService(IHubContext<ServiceDeskHub> hubContext, IConnectionManager connectionManager)
        {
            _hubContext = hubContext;
            //_context = connectionManager.GetHubContext<ServiceDeskHub>();
        }

        public async Task PublishForAllClientsAsync(IEvent @event)
        {
            await _hubContext.Clients.All.InvokeAsync(GetEventName(@event), @event);

            Logger.Debug($"PublishForAllClients: {GetEventName(@event)}");
        }
        public async Task PublishForOneClientsAsync(IAuthenticatedEvent @event)
        {
            await _hubContext.Clients.Client(@event.UserId).InvokeAsync(GetEventName(@event), @event);

            Logger.Debug($"PublishForOneClients: {GetEventName(@event)}");
        }

        private string GetEventName(IEvent @event) 
            => @event.GetType().Name.Humanize(LetterCasing.LowerCase).Underscore();
    }
}