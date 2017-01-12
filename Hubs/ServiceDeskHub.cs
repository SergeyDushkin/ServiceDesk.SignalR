using System.Threading.Tasks;
using Coolector.Common.Security;
using Microsoft.AspNetCore.SignalR;
using NLog;

namespace servicedesk.SignalR.Hubs
{
    public class ServiceDeskHub : Hub
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IJwtTokenHandler jwtTokenHandler;

        public ServiceDeskHub(IJwtTokenHandler jwtTokenHandler)
        {
            this.jwtTokenHandler = jwtTokenHandler;
        }

        public override Task OnConnectedAsync()
        {
            Logger.Debug($"Connected to Hub, connectionId:{Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync()
        {
            Logger.Debug($"Disconnected from hub, connectionId:{Context.ConnectionId}");
            return base.OnDisconnectedAsync();
        }

        public async Task InitializeAsync(string header)
        {
            //var token = Startup.JwtTokenHandler.GetFromAuthorizationHeader(header);
            //if (Startup.JwtTokenHandler.IsValid(token) == false)

            var token = jwtTokenHandler.GetFromAuthorizationHeader(header);
            if (jwtTokenHandler.IsValid(token) == false)
            {
                Logger.Debug("Authorization token is invalid, disconnecting client");
                await Clients.Client(Context.ConnectionId).InvokeAsync("disconnect");
                return;
            }
            var userId = token.Sub;
            Logger.Debug($"Assigning ConnectionId:{Context.ConnectionId} to user:{userId}");
            await Groups.AddAsync(userId);
        }
    }
}