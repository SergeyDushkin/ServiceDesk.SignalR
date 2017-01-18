using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using NLog;

namespace servicedesk.SignalR.Hubs
{
    public class RawConnection : PersistentConnection
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public RawConnection()
        {
        }

        protected override async Task OnConnected(HttpRequest request, string connectionId)
        {
            Logger.Debug($"Connected to Hub, connectionId:{connectionId}");
            await base.OnConnected(request, connectionId);
        }

        protected override async Task OnDisconnected(HttpRequest request, string connectionId, bool stopCalled)
        {
            Logger.Debug($"Disconnected from hub, connectionId:{connectionId}");
            await base.OnDisconnected(request, connectionId, stopCalled);
        }
    }
}