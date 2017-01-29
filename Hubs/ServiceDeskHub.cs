using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNetCore.SignalR.Hubs;
using NLog;

namespace servicedesk.SignalR.Hubs
{
    //[HubName("servicedeskhub")]
    public class ServiceDeskHub : Hub
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //private readonly IJwtTokenHandler jwtTokenHandler;

        //public ServiceDeskHub(IJwtTokenHandler jwtTokenHandler)
        public ServiceDeskHub()
        {
            //this.jwtTokenHandler = jwtTokenHandler;
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
            /*
            var token = Startup.JwtTokenHandler.GetFromAuthorizationHeader(header);
            if (Startup.JwtTokenHandler.IsValid(token) == false)
            {
                Logger.Debug("Authorization token is invalid, disconnecting client");
                await Clients.Client(Context.ConnectionId).InvokeAsync("disconnect");
                return;
            }
            var userId = token.Sub;
            Logger.Debug($"Assigning ConnectionId:{Context.ConnectionId} to user:{userId}");
            await Groups.AddAsync(userId);*/
        }

        /*

        public override Task OnConnected()
        {
            Logger.Debug($"Connected to Hub, connectionId:{Context.ConnectionId}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Logger.Debug($"Disconnected from hub, connectionId:{Context.ConnectionId}");
            return base.OnDisconnected(stopCalled);
        }
        */


        /*
        public async Task InitializeAsync(string header)
        {
            //var token = Startup.JwtTokenHandler.GetFromAuthorizationHeader(header);
            //if (Startup.JwtTokenHandler.IsValid(token) == false)

            //var token = jwtTokenHandler.GetFromAuthorizationHeader(header);
            //if (jwtTokenHandler.IsValid(token) == false)
            //{
            //    Logger.Debug("Authorization token is invalid, disconnecting client");
            //    await Clients.Client(Context.ConnectionId).InvokeAsync("disconnect");
            //    return;
            //}
            //var userId = token.Sub;
            //Logger.Debug($"Assigning ConnectionId:{Context.ConnectionId} to user:{userId}");
            //await Groups.Add(userId);
        }*/
    }
}